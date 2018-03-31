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
        public User CurrentUser { get; set; }
        public string RefreshToken { get; set; }
        public bool StayLogged => !string.IsNullOrEmpty(RefreshToken);

        public string Token { get; set; }
        public bool IsLogged => CurrentUser != null;
        public int UserRank => IsLogged ? CurrentUser.RankLevel : ZAL.RANK.NOVACEK;

        private static SessionGateway gateway;
        private static SessionGateway Gateway => gateway ?? (gateway = new SessionGateway());

        internal JObject GetJson() {
            JObject json = new JObject {
                { "StayLogged", StayLogged }
            };
            if (StayLogged) {
                json.Add("RefreshToken", RefreshToken);
                json.Add("CurrentUser", JToken.FromObject(CurrentUser));
            }            
            return json;
        }

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

        public async Task AskForNewToken() {
            if (StayLogged && CurrentUser != null) {
                var requestModel = new TokenRequestModel {
                    IdUser = CurrentUser.Id,
                    RefreshToken = RefreshToken,
                };
                var respondModel = await Gateway.RefreshToken(requestModel);
                if (!respondModel.IsExpired) {
                    Token = respondModel.Token;
                }
                else {
                    Clear();
                }
            }

        }

        /*public static void Logout() {
            //Me = User.Empty();
            Session.Stop();
        }*/
    }
}
