using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;
using ZalDomain.consts;

namespace ZalDomain
{
    public class Session
    {
        public User CurrentUser { get; set; }
        public string RefreshToken { get; set; }
        public bool StayLogged { get; set; } = false;

        public string Token { get; set; }
        public bool IsLogged => CurrentUser != null;
        public int UserRank => IsLogged ? CurrentUser.RankLevel : ZAL.RANK.CLEN;

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

        /*public static async Task<bool> LoginAsync(string email, string password) {
            bool isLogged = false;
            if (IsConnected) {
                if (Users.Contains(email)) {
                    User user = await Users.LoginAsync(email, password);
                    if (user != null) {
                        Session.CurrentUser = user;
                        isLogged = true;
                        ReSynchronizeAsync();
                    }
                }
            }
            return isLogged;
        }

        public static async Task<bool> RegisterAsync(string name, string surname, string phone, string email, string password) {
            bool isRegistered = false;
            if (IsConnected) {
                if (!Users.Contains(email)) {
                    User user = await Users.RegisterNewUserAsync(name, surname, phone, email, password);
                    if (user != null) {
                        Session.CurrentUser = user;
                        isRegistered = true;
                    }
                }
            }
            return isRegistered;
        }

        public static void Logout() {
            //Me = User.Empty();
            Session.Stop();
        }*/
    }
}
