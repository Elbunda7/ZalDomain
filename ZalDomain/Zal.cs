﻿using ZalDomain.ActiveRecords;
using ZalDomain.ItemSets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ZalDomain
{
    public delegate void OfflineCommandDelegate(XDocument commands);

    public static class Zal
    {
        public static event OfflineCommandDelegate CommandExecutedOffline;


        public static bool IsConnected { get; private set; } = true;
        public static bool UserIsLogged => Session.IsLogged;

        public static Session Session { get; set; } = new Session();

        public static DocumentSet Documents { get; private set; } = new DocumentSet();
        public static BadgeSet Badges { get; private set; } = new BadgeSet();
        public static UserSet Users { get; private set; } = new UserSet();
        public static ActualitySet Actualities { get; private set; } = new ActualitySet();
        public static ActionSet Actions { get; private set; } = new ActionSet();

        [Obsolete]
        public static async Task<bool> LoginAsync(string email, string password) {
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

        [Obsolete]
        public static bool LoginAsGuest() {
            if (IsConnected) {
                //Me = User.Empty();
                Session = new Session();
                ReSynchronizeAsync();
            }
            return false;
        }

        [Obsolete]
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
        [Obsolete]
        public static void Logout() {
            //Me = User.Empty();
            Session.Stop();
        }

        public static void LoadOfflineCommands(XDocument commands) {
            //Database.LoadOfflineCommands(commands);
        }

        //public static bool Connect() {
        //    IsConnected = Database.Connect(CONNECTION_STRING);
        //    if (IsConnected) {//???
        //        StartSynchronizing();
        //    }
        //    return IsConnected;
        //}

        private static void OnCommandExecutedOffline(XDocument command) {
            if (CommandExecutedOffline != null) {
                CommandExecutedOffline.Invoke(command);
            }
            else {
                throw new Exception("Command is executed when offline, but no-one is listening this event");
            }
        }

        public static async Task StartSynchronizingAsync() {
            //Documents.Synchronize();
            //Badgets.Synchronize();
            //Users.Synchronize();
            //Actualities.Synchronize();
            await Actions.SynchronizeAsync();
        }

        private static async Task ReSynchronizeAsync() {
            Documents.ReSynchronize();
            Badges.ReSynchronize();
            Users.ReSynchronize();
            Actualities.ReSynchronize();
            await Actions.ReSynchronizeAsync();
        }

        public static void LoadDataFrom(string json) {
            try {
                JObject jObject = JObject.Parse(json);
                Session = JsonConvert.DeserializeObject<Session>(jObject.GetValue("session").ToString());
                Actions.LoadFrom(jObject.GetValue("actions"));
            }
            catch (Exception) {
            }
        }

        public static string GetDataJson() {
            JObject jObject = new JObject {
                {"session", Session.GetJson() },
                {"actions", Actions.GetJson() }
            };
            return jObject.ToString();
        }
    }
}
