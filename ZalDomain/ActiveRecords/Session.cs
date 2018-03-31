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
        public bool StayLogged { get; set; } = false;

        public string Token { get; set; }
        public bool IsLogged => CurrentUser != null;
        public int UserRank => IsLogged ? CurrentUser.RankLevel : ZAL.RANK.CLEN;

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

        internal void Stop() {
            CurrentUser = null;
            Token = null;
        }

        public async Task<LoginErrorModel> LoginAsync(string email, string password, bool stayLogged) {
            var requestModel = new LoginRequestModel {
                Email = email,
                Password = password
            };
            var respondModel = await Gateway.LoginAsync(requestModel);
            if (!respondModel.HasAnyErrors) {
                CurrentUser = new User(respondModel.UserModel);
                Token = respondModel.Token;
                RefreshToken = respondModel.RefreshToken;
                StayLogged = stayLogged;
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

        /*public static void Logout() {
            //Me = User.Empty();
            Session.Stop();
        }*/
    }
}
