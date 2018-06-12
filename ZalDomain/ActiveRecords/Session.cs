using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway;
using ZalDomain.consts;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalApiGateway.Models;
using ZalDomain.Models;

namespace ZalDomain.ActiveRecords
{
    public class Session
    {
        public event SessionStateDelegate UsersSessionStateChanged;

        public User CurrentUser { get; set; }
        private string RefreshToken { get; set; }
        private string Token { get; set; }

        public bool StayLogged => !string.IsNullOrEmpty(RefreshToken);
        public bool IsUserLogged => CurrentUser != null;
        public int UserRank => IsUserLogged ? CurrentUser.RankLevel : ZAL.RANK.NOVACEK;

        private static SessionGateway gateway;
        private static SessionGateway Gateway => gateway ?? (gateway = new SessionGateway());


        private void Clear() {
            CurrentUser = null;
            RefreshToken = null;
            Token = null;
        }

        internal void Stop() {
            CurrentUser = null;
            Token = null;
        }

        public async Task<LoginErrorModel> LoginAsync(string email, string password, bool stayLogged) {
            var requestModel = new LoginRequestModel {
                Email = email,
                Password = password, 
                StayLogged = stayLogged
            };
            var respondModel = await Gateway.LoginAsync(requestModel);
            if (!respondModel.HasAnyErrors) {
                CurrentUser = new User(respondModel.UserModel);
                Token = respondModel.Token;
                RefreshToken = respondModel.RefreshToken;
                RaisSessionStateChanged();
            }
            return new LoginErrorModel(respondModel);
        }

        public async Task<bool> RegisterAsync(string name, string surname, string phone, string email, string password) {
            var requestModel = new RegistrationRequestModel {
                Name = name,
                Surname = surname,
                Phone = phone,
                Email = email,
                Password = password
            };
            bool isRegistered = await Gateway.RegisterAsync(requestModel);
            if (isRegistered) {
                CurrentUser = new User(new UserModel(requestModel));
            }
            return isRegistered;
        }

        public async Task<bool> TryLoginWithTokenAsync() {
            bool isLogged = StayLogged && CurrentUser != null;
            if (isLogged) {
                var requestModel = new TokenRequestModel {
                    IdUser = CurrentUser.Id,
                    RefreshToken = RefreshToken,
                };
                var respondModel = await Gateway.RefreshTokenAsync(requestModel);
                isLogged = !respondModel.IsExpired;
                if (isLogged) {
                    Token = respondModel.Token;
                    RaisSessionStateChanged();
                }
                else {
                    Clear();
                }
            }
            return isLogged;
        }

        public void RaisSessionStateChanged() {
            if (UsersSessionStateChanged != null) {
                UsersSessionStateChanged.Invoke(this);
            }
        }

        public async Task Logout() {
            var requestModel = new LogoutRequestModel {
                IdUser = CurrentUser.Id,
                Token = Token,
            };
            await Gateway.LogoutAsync(requestModel);
            Clear();
        }

        internal JObject GetJson() {
            JObject json = new JObject {
                { "StayLogged", StayLogged }
            };
            if (StayLogged) {
                json.Add("RefreshToken", RefreshToken);
                json.Add("CurrentUser", CurrentUser.GetModelJson());
            }            
            return json;
        }

        internal void LoadFrom(JToken jToken) {
            bool stayLogged = jToken.Value<bool>("StayLogged");
            if (stayLogged) {
                RefreshToken = jToken.Value<string>("RefreshToken");
                CurrentUser = User.LoadFrom(jToken.SelectToken("CurrentUser"));
            }
        }
    }
}
