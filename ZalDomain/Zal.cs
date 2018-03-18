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

    public static class Zal {

        public static event OfflineCommandDelegate CommandExecutedOffline;

        private const string CONNECTION_STRING = 
            "server=dbsys.cs.vsb.cz\\STUDENT;database=bre0118;user=bre0118;password=BCQSpQY5YQ;";
        //"Data Source = Kluci-PC\\SQLEXPRESS;Initial Catalog = master; Integrated Security = True";

        private static DAL.Database database;
        private static DAL.Database Database  {
            get {
                if (database == null) {
                    database = DAL.Database.GetInstance();
                    database.CommandExecutedOffline += OnCommandExecutedOffline;
                }
                return database;
            }
        }


        public static bool IsConnected { get; private set; } = false;
        public static bool UserIsLogged {get {return Me.Email != "";}}

        public static User Me { get; set; } = User.Empty();

        public static DocumentSet Documents { get; private set; } = new DocumentSet();
        public static BadgeSet Badgets { get; private set; } = new BadgeSet();
        public static UserSet Users { get; private set; } = new UserSet();
        public static ActualitySet Actualities { get; private set; } = new ActualitySet();
        public static ActionSet Actions { get; private set; } = new ActionSet();


        public static bool Login(string email, string password) {
            bool isLogged = false;
            if (IsConnected) {
                if (Users.Contains(email)) {
                    User user = Users.Login(email, password);
                    if (user != null) {
                        Me = user;
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
                Me = User.Empty();
                ReSynchronize();
            }
            return false;
        }

        public static bool Register(string name, string surname, string phone, string email, string password) {
            bool isRegistered = false;
            if (IsConnected) {
                if (!Users.Contains(email)) {
                    User user = Users.RegisterNewUser(name, surname, phone, email, password);
                    if (user != null) {
                        Me = user;
                        isRegistered = true;
                    }
                }
            }
            return isRegistered;
        }

        public static void Logout() {
            Me = User.Empty();
        }

        public static void LoadOfflineCommands(XDocument commands) {
            Database.LoadOfflineCommands(commands);
        }

        public static bool Connect() {
            IsConnected = Database.Connect(CONNECTION_STRING);
            if (IsConnected) {//???
                Synchronize();
            }
            return IsConnected;
        }

        private static void OnCommandExecutedOffline(XDocument command) {
            if (CommandExecutedOffline != null) {
                CommandExecutedOffline.Invoke(command);
            }
            else {
                throw new Exception("Commant is executed when offline, but no-one is listening this event");
            }
        }

        public static void Synchronize() {
            Documents.Synchronize();
            Badgets.Synchronize();
            Users.Synchronize();
            Actualities.Synchronize();
            Actions.Synchronize();
        }

        private static void ReSynchronize() {
            Documents.ReSynchronize();
            Badgets.ReSynchronize();
            Users.ReSynchronize();
            Actualities.ReSynchronize();
            Actions.ReSynchronize();
        }

        public static XDocument GetLocalDataXml() {
            XDocument doc = new XDocument();
            XElement root = new XElement("root");
            root.Add(Actualities.GetXml("Actualities"));
            root.Add(Actions.GetXml("Actions"));
            //root.Add(Docs.GetXml("Docs"));
            doc.Add(root);
            return doc;
        }

        public static void LoadLocalData(XDocument doc) {
            if (doc != null) {
                XElement root = doc.Root;
                Actualities.LoadFromXml(root.Element("Actualities"));
                Actions.LoadFromXml(root.Element("Actions"));
                //Docs.LoadFromXml(root.Element("Docs"))
            }
        }
    }
}
