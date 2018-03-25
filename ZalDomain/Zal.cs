using ZalDomain.ActiveRecords;
using ZalDomain.ItemSets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ZalDomain
{
    public delegate void OfflineCommandDelegate(XDocument commands);

    public static class Zal
    {
        public static event OfflineCommandDelegate CommandExecutedOffline;


        public static bool IsConnected { get; private set; } = false;
        public static bool UserIsLogged => Session.IsLogged;

        public static Session Session { get; set; } = new Session();

        public static DocumentSet Documents { get; private set; } = new DocumentSet();
        public static BadgeSet Badges { get; private set; } = new BadgeSet();
        public static UserSet Users { get; private set; } = new UserSet();
        public static ActualitySet Actualities { get; private set; } = new ActualitySet();
        public static ActionSet Actions { get; private set; } = new ActionSet();


        public static async Task<bool> LoginAsync(string email, string password) {
            bool isLogged = false;
            if (IsConnected) {
                if (Users.Contains(email)) {
                    User user = await Users.LoginAsync(email, password);
                    if (user != null) {
                        Session.CurrentUser = user;
                        isLogged = true;
                        ReSynchronize();
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
                ReSynchronize();
            }
            return false;
        }

        public static async Task<bool> Register(string name, string surname, string phone, string email, string password) {
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

        public static void StartSynchronizing() {
            //Documents.Synchronize();
            //Badgets.Synchronize();
            //Users.Synchronize();
            //Actualities.Synchronize();
            Actions.Synchronize();
        }

        private static void ReSynchronize() {
            Documents.ReSynchronize();
            Badges.ReSynchronize();
            Users.ReSynchronize();
            Actualities.ReSynchronize();
            Actions.ReSynchronize();
        }

        public static XDocument GetLocalDataXml() {
            XDocument doc = new XDocument();
            /*XElement root = new XElement("root");
            root.Add(Actualities.GetXml("Actualities"));
            root.Add(Actions.GetXml("Actions"));
            //root.Add(Docs.GetXml("Docs"));
            doc.Add(root);*/
            return doc;
        }

        public static void LoadLocalData(XDocument doc) {
            /*if (doc != null) {
                XElement root = doc.Root;
                Actualities.LoadFromXml(root.Element("Actualities"));
                Actions.LoadFromXml(root.Element("Actions"));
                //Docs.LoadFromXml(root.Element("Docs"))
            }*/
        }
    }
}
