using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Gateway;
using DAL.tableStructures;
using ZalDomain.consts;

namespace ZalDomain.ActiveRecords
{
    public class User : IActiveRecord {
        private UzivatelMainTable MainData;
        private UzivatelDetailTable otherData;
        private UzivatelDetailTable OtherData { get { return OtherDataLazyLoad(); } set { otherData = value; } }
        private Collection<Badge> budgets;

        public string Email { get { return MainData.Email; } }
        public string ShortName { get { return MainData.ShortName; } }
        public int RankLevel { get { return MainData.Hodnost; } }
        public int GroupNumber { get { return MainData.Id_druzina; } }
        public string Rank { get { return ZAL.RANK_NAME[MainData.Hodnost]; } }
        public string Group { get { return ZAL.GROUP_NAME_SING[MainData.Id_druzina]; } }
        public string Name { get { return OtherData.Jmeno; } }
        public string Surname { get { return OtherData.Prijmeni; } }
        public string Nick { get { return OtherData.Prezdivka; } } //nick == shortName ?
        public string Phone { get { return OtherData.Kontakt; } }
        public string Role { get { return OtherData.Role; } }
        public int Points { get { return OtherData.Body; } }
        public DateTime DateOfBirth { get { return OtherData.Datum_narozeni; } }
        public bool PaidForMembership { get { return OtherData.Zaplatil_prispevek; } }
        public Collection<Badge> Budgets { get { return BudgetsLazyLoad(); } private set { budgets = value; } }


        internal static User AddNewEmptyUser(string name, string surname, int group) {
            User user = new User("", name, surname, "", group);
            UzivatelTable data = new UzivatelTable(user.MainData, user.OtherData);
            if (Gateway.InsertEmptyMember(data)) {
                return user;
            }
            return null;
        }

        private bool IsChanged { get; set; }

        private static UzivatelGateway gateway;
        private static UzivatelGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = UzivatelGateway.GetInstance();
                }
                return gateway;
            }
        }


        private UzivatelDetailTable OtherDataLazyLoad() {
            if (otherData == null) {
                otherData = Gateway.SelectDetail(MainData.Email);
            }
            return otherData;
        }

        private Collection<Badge> BudgetsLazyLoad() {
            if (budgets == null) {
                budgets = Zal.Badgets.GetAcquired();
            }
            return budgets;
        }



        public User(UzivatelMainTable item) {
            MainData = item;
        }

        internal User(UzivatelTable item) {
            MainData = item.Main;
            OtherData = item.Minor;
        }

        private User(string email, string jmeno, string prijmeni, string kontakt, int group = ZAL.GROUP.NON) {
            MainData = new UzivatelMainTable {
                Email = email,
                Hodnost = 1,
                Id_druzina = group,
            };
            OtherData = new UzivatelDetailTable {
                Jmeno = jmeno,
                Prijmeni = prijmeni,
                Kontakt = kontakt,
                Prezdivka = null,
                Role = ZAL.MEMBERSHIP.NECLEN,
                Body = 0,
                Zaplatil_prispevek = false,
                //Datum_narozeni, Pres_facebook
            };
            SetShortName();
        }

        public static User Login(string email, string password) {
            User user = null;
            if (gateway.Login(email, password)) {
                user = new User(gateway.SelectFull(email));
            }
            return user;
        }

        public static User Empty() {
            return new User("", "", "", "");
        }


        public bool Has(int key, int value) {
            switch (key) {
                case CONST.USER.DRUZINA: return MainData.Id_druzina == value;
                default: return false;
            }
        }

        public bool Has(int key, String value) {
            switch (key) {
                case CONST.USER.EMAIL: return value.Equals(MainData.Email);
                case CONST.USER.JMENO: return value.Equals(MainData.ShortName);
                default: return false;
            }
        }

        public static Collection<User> GetAllMembers() {
            Collection<User> users = new Collection<User>();
            foreach (UzivatelMainTable u in Gateway.GetAllMembers()) {
                users.Add(new User(u));
            }
            return users;
        }

        public static bool CheckForChanges(int userCount, DateTime lastCheck) {
            return Gateway.CheckForChanges(userCount, lastCheck);
        }

        public static User Select(string email) {
            return new User(Gateway.SelectFull(email));
        }

        public static bool Contains(string email) {
            return Gateway.Contains(email);
        }

        public static User RegisterNew(string email, string jmeno, string prijmeni, string kontakt, string password) {
            User user = new User(email, jmeno, prijmeni, kontakt);
            UzivatelTable data = new UzivatelTable(user.MainData, user.OtherData);
            if (Gateway.Insert(data, password)) {
                return user;
            }
            return null;
        }

        public void BecomeMember(DateTime dateOfBirthDay, string prezdivka = null) {
            if (OtherData.Role == ZAL.MEMBERSHIP.NECLEN) {
                OtherData.Datum_narozeni = dateOfBirthDay;
                OtherData.Prezdivka = prezdivka;
                OtherData.Role = ZAL.MEMBERSHIP.CLEN;
                MainData.Id_druzina = ZAL.GROUP.BOBRI;
                Gateway.BecomeMember(new UzivatelTable(MainData, OtherData));
            }
            else {
                throw new Exception("user already is a member");
            }
        }

        /*public static bool Has(int key, int value) {
            switch (key) {
                case DRUZINA: return Id_druzina == value;
                default: return false;
            }
        }

        internal bool Has(int key, String value) {
            switch (key) {
                case EMAIL: return value.Equals(Email);
                case JMENO: return value.Equals(Jmeno);
                case PRIJMENI: return value.Equals(Prijmeni);
                case PREZDIVKA: return value.Equals(Prezdivka);
                case ROLE: return value.Equals(Role);
                default: return false;
            }
        }*/

        public void ConfirmChanges() {
            if (IsChanged) {
                Gateway.Update(new UzivatelTable(MainData, OtherData));
                IsChanged = false;
            }
        }

        public void Update(int key, int value) {
            switch (key) {
                case CONST.USER.DRUZINA: ChangeGroup(value); break;
                case CONST.USER.HODNOST: ChangeRank(value); break;
                case CONST.USER.BODY: ChangePoints(value); break;
            }
        }

        public void Update(int key, String value) {
            switch (key) {
                case CONST.USER.EMAIL: ChangeEmail(value); break;
                case CONST.USER.JMENO: ChangeName(value); break;
                case CONST.USER.PRIJMENI: ChangeSurname(value); break;
                case CONST.USER.PREZDIVKA: ChangeNick(value); break;
                case CONST.USER.ROLE: ChangeRole(value); break;
                case CONST.USER.KONTAKT: ChangePhone(value); break;
            }
        }

        public void Update(int key, bool value) {
            switch (key) {
                case CONST.USER.ZAPLATIL_PRISPEVEK: ChangeIsPaid(value); break;
            }
        }

        private void ChangeIsPaid(bool value) {
            if (OtherData.Zaplatil_prispevek != value) {
                OtherData.Zaplatil_prispevek = value;
                Gateway.Update(MainData.Email, value);
            }
        }

        private void ChangeName(string value) {
            if (OtherData.Jmeno != value) {
                OtherData.Jmeno = value;
                SetShortName();
                IsChanged = true;
            }
        }

        private void ChangeSurname(string value) {
            if (OtherData.Prijmeni != value) {
                OtherData.Prijmeni = value;
                SetShortName();
                IsChanged = true;
            }
        }

        private void ChangeNick(string value) {
            if (OtherData.Prezdivka != value) {
                OtherData.Prezdivka = value;
                SetShortName();
                IsChanged = true;
            }
        }

        private void ChangePhone(string value) {
            if (OtherData.Kontakt != value) {
                OtherData.Kontakt = value;
                IsChanged = true;
            }
        }

        private void ChangePoints(int value) {
            throw new NotImplementedException();
        }

        private void ChangeEmail(string value) {
            throw new NotImplementedException();
        }

        private void ChangeRole(string value) {
            if (OtherData.Role != value) {
                if (value == ZAL.MEMBERSHIP.CLEN) {
                    if (OtherData.Datum_narozeni == DateTime.MinValue) {
                        return;
                    }
                }
                OtherData.Role = value;
                IsChanged = true;
            }
        }

        private void ChangeGroup(int druzina) {
            if (druzina != MainData.Id_druzina) {
                if (druzina == ZAL.GROUP.LISKY) {
                    MainData.Id_druzina = druzina;
                    MainData.Hodnost = ZAL.RANK.LISKA;
                }
                else if (druzina == ZAL.GROUP.TROSKY) {
                    MainData.Id_druzina = druzina;
                    if (MainData.Hodnost < ZAL.RANK.VEDOUCI) {
                        MainData.Hodnost = ZAL.RANK.VEDOUCI;
                    }
                }
                else {
                    MainData.Id_druzina = druzina;
                    if (MainData.Hodnost < ZAL.RANK.NOVACEK || MainData.Hodnost > ZAL.RANK.RADCE) {
                        MainData.Hodnost = ZAL.RANK.NOVACEK;
                    }
                }
                IsChanged = true;
            }
        }

        private void ChangeRank(int hodnost) {
            if (hodnost != MainData.Hodnost) {
                if (hodnost == ZAL.RANK.LISKA) {
                    MainData.Id_druzina = ZAL.GROUP.LISKY;
                    MainData.Hodnost = hodnost;
                }
                else if (hodnost >= ZAL.RANK.VEDOUCI) {
                    MainData.Id_druzina = ZAL.GROUP.TROSKY;
                    MainData.Hodnost = hodnost;
                }
                else {
                    MainData.Hodnost = hodnost;
                    if (MainData.Id_druzina == ZAL.GROUP.LISKY || MainData.Id_druzina == ZAL.GROUP.TROSKY) {
                        MainData.Id_druzina = ZAL.GROUP.BOBRI;
                    }
                }
                IsChanged = true;
            }
        }

        private void SetShortName() {
            if (OtherData.Prezdivka != null) {
                MainData.ShortName = OtherData.Prezdivka;
            }
            else {
                if (OtherData.Prijmeni.Length == 0) {
                    MainData.ShortName = OtherData.Jmeno;
                }
                else {
                    MainData.ShortName = OtherData.Jmeno + OtherData.Prijmeni[0] + '.';
                }
            }
        }

        public bool ChangePassword(string userEmail, string oldPass, string newPass) {
            return Gateway.UpdatePassword(userEmail, oldPass, newPass);
        }

        public void AddBudget(Badge budget) {
            Budgets.Add(budget);
            Gateway.InsertBadget(Email, budget.Id);
        }

        /*public static User GetUser(string email) {
            return Gateway.Select(email);
        }*/

        public static bool Exists(string email) {
            return Gateway.Contains(email);
        }

        public override string ToString() {
            return ShortName;
        }

        public bool IsLeader() {
            return RankLevel >= ZAL.RANK.VEDOUCI;
        }
    }
}
