using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using ZalDomain.consts;
using DAL.tableStructures;
using DAL.Gateway;
using System.Xml.Linq;
using ZalDomain.tools;

namespace ZalDomain.ActiveRecords
{
    public class ActionEvent : IActiveRecord
    {
        private AkceTable Data;
        private User garant;
        private Collection<User> participants;
        private InfoAction info;
        private RecordAction record;

        public int Id { get { return Data.Id; } }
        public string Type { get { return Data.Typ; } }
        public string Name { get { return Data.Jmeno; } }
        public int DayCount { get { return Data.Pocet_dni; } }
        public int RankLeast { get { return Data.Od_hodnosti; } }
        public DateTime DateFrom { get { return Data.Datum_od; } }
        public User Garant { get { return GarantLazyLoad(); } set { SetGarant(value); } }
        public InfoAction Info { get { return InfoLazyLoad(); } private set { info = value; } }
        public RecordAction Record { get { return RecordLazyLoad(); } private set { record = value; } }
        public Collection<User> Participants { get { return ParticipantsLazyLoad(); } private set { participants = value; } }
        public int ParticipantsCount { get { return Participants.Count; } }//Data.NumOfParticipants
        //public bool Je_oficialni { get; set; }
        //účastním se?



        private InfoAction InfoLazyLoad() {
            if (info == null) {
                info = Zal.Actualities.GetInfoForAction(Data.Id);
            }
            return info;
        }

        private RecordAction RecordLazyLoad() {
            if (record == null) {
                record = Zal.Actualities.GetZapisForAction(Data.Id);
            }
            return record;
        }


        private static AkceGateway gateway;
        private static AkceGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = AkceGateway.GetInstance();
                }
                return gateway;
            }
        }

        public ActionEvent(string jmeno, string typ, DateTime od, int pocetDni, int odHodnosti, bool jeOficialni) {
            Data = new AkceTable {
                Id = -1,
                Jmeno = jmeno,
                Typ = typ,
                Datum_od = od,
                Pocet_dni = pocetDni,
                Email_vedouci = null,
                Od_hodnosti = odHodnosti,
                Je_oficialni = jeOficialni,
            };
            Gateway.Insert(Data);
        }

        public ActionEvent(AkceTable data) {
            Data = data;
        }

        private User GarantLazyLoad() {
            if (garant == null) {
                if (Data.Email_vedouci != null) {
                    garant = Zal.Users.GetByEmail(Data.Email_vedouci);
                }
                else {
                    garant = User.Empty();
                }               
            }
            return garant;
        }

        private Collection<User> ParticipantsLazyLoad() {
            if (participants == null) {//obnovit?
                List<string> list = gateway.GetParticipatingAt(Data.Id);
                participants = Zal.Users.GetByEmailList(list);
            }
            return participants;
        }

        public bool Has(int key, int value) {
            switch (key) {
                case CONST.AKCE.ID: return Data.Id == value;
                case CONST.AKCE.POCET_DNI: return Data.Pocet_dni == value;
                case CONST.AKCE.OD_HODNOSTI: return Data.Od_hodnosti == value;
                default: return false;
            }
        }

        public bool Has(int key, String value) {
            switch (key) {
                case CONST.AKCE.JMENO: return value.Equals(Data.Jmeno);
                case CONST.AKCE.TYP: return value.Equals(Data.Typ);
                case CONST.AKCE.EMAIL_VEDOUCI: return value.Equals(Data.Email_vedouci);
                default: return false;
            }
        }

        public bool Has(int key, bool value) {
            switch (key) {
                case CONST.AKCE.JE_OFICIALNI: return Data.Je_oficialni == value;
                default: return false;
            }
        }

        public bool Has(int key, DateTime value) {
            switch (key) {
                case CONST.AKCE.DATUM: return Data.Datum_od.Date == value.Date;
                case CONST.AKCE.DATUM_DO: return Data.Datum_od.Date <= value.Date;
                case CONST.AKCE.DATUM_OD: return Data.Datum_od.Date >= value.Date;
                default: return false;
            }
        }

        public bool SetGarant(User uzivatel) {
            IntegrityCondition.UserIsLeader();
            if (Gateway.JoinLeaderToAction(uzivatel.Email, Data.Id)) {
                Garant = uzivatel;
                return true;
            }
            return false;
        }

        public void Aktualize(String jmeno, String typ, DateTime? od, int? pocetDni, int? odHodnosti, bool? jeOficialni) {
            IntegrityCondition.UserIsLeader();
            if (jmeno != null) {
                Data.Jmeno = jmeno;
            }
            if (typ != null) {
                Data.Typ = typ;
            }
            if (od != null) {
                Data.Datum_od = (DateTime)od;
            }
            if (pocetDni != null) {
                Data.Pocet_dni = (int)pocetDni;
            }
            if (odHodnosti != null) {
                Data.Od_hodnosti = (int)odHodnosti;
            }
            if (jeOficialni != null) {
                Data.Je_oficialni = (bool)jeOficialni;
            }
            Gateway.Update(Data);
        }

        public int GetNumberOfMembers() {
            throw new NotImplementedException();
            //return Gateway.getNumOfUsers(this);
        }

        public void Participate(User user, bool isGoing) {
            Gateway.Participate(user.Email, isGoing, Data.Id);
        }

        public override string ToString() {
            return Data.Jmeno;
        }

        public static string CheckForChanges(User user, DateTime lastCheck) {
            return Gateway.CheckForChanges(user.RankLevel, lastCheck);
        }

        public static Collection<ActionEvent> GetUpcoming(User user) {
            Collection<ActionEvent> upcomingActions = new Collection<ActionEvent>();
            bool onlyOfficial = user.RankLevel >= ZAL.RANK.VEDOUCI ? false : true;
            Collection<AkceTable> actions = Gateway.GetUpcoming(user.RankLevel, onlyOfficial);
            foreach (AkceTable a in actions) {
                upcomingActions.Add(new ActionEvent(a));
            }
            return upcomingActions;
        }

        public static List<int> GetChanged(User user, DateTime lastCheck) {
            return Gateway.GetChanged(user.RankLevel, lastCheck);
        }

        public static ActionEvent Get(int id) {
            return new ActionEvent(Gateway.Get(id));
        }

        public static bool Delete(ActionEvent akce) {
            return 1 <= Gateway.Delete(akce.Data.Id);
        }

        public XElement GetXml(string elementName) {
            XElement element = new XElement(elementName,
                new XElement("Id", Data.Id),
                new XElement("DateOfAction", Data.Datum_od.Ticks),
                new XElement("Name", Data.Jmeno),
                new XElement("Type", Data.Typ),
                new XElement("IsOfficial", Data.Je_oficialni),
                new XElement("FromRank", Data.Od_hodnosti),
                new XElement("Days", Data.Pocet_dni),
                new XElement("GarantEmail",Data.Email_vedouci));
            return element;
        }

        public static ActionEvent LoadFromXml(XElement element) {
            AkceTable data = new AkceTable {
                Id = Int32.Parse(element.Element("Id").Value),
                Datum_od = new DateTime(long.Parse(element.Element("DateOfAction").Value)),
                Jmeno = element.Element("Name").Value,
                Typ = element.Element("Type").Value,
                Je_oficialni = Boolean.Parse(element.Element("IsOfficial").Value),
                Od_hodnosti = Int32.Parse(element.Element("FromRank").Value),
                Pocet_dni = Int32.Parse(element.Element("Days").Value),
            };
            if (!element.Element("GarantEmail").IsEmpty) {
                data.Email_vedouci = element.Element("GarantEmail").Value;
            }
            return new ActionEvent(data);
        }
    }
}
