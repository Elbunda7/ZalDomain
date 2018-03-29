using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.consts;

namespace ZalDomain.ActiveRecords
{
    public class User : IActiveRecord {

        private UserModel model;
        //private UzivatelMainTable model;
        //private UzivatelDetailTable otherData;
        //private UzivatelDetailTable model { get { return OtherDataLazyLoad(); } set { otherData = value; } }
        private Collection<Badge> budges;

        public int Id => model.Id;
        public string Email => model.Email;
        public string Nick => model.NickName;
        public int RankLevel => model.Id_Rank;
        public int? GroupNumber => model.Id_Group;
        //public string Rank { get { return ZAL.RANK_NAME[model.Hodnost]; } }
        //public string Group { get { return ZAL.GROUP_NAME_SING[model.Id_druzina]; } }
        public string Name => model.Name;
        public string Surname => model.Surname;
        public string Phone => model.Phone;
        //public string Role { get { return model.Role; } }
        //public int Points { get { return model.Body; } }
        public DateTime? DateOfBirth { get { return model.BirthDate; } }
        //public bool PaidForMembership { get { return model.Zaplatil_prispevek; } }
        //public Collection<Badge> Budges { get { return BudgesLazyLoad(); } private set { budgets = value; } }


        internal static User AddNewEmptyUser(string name, string surname, int group) {
            UserModel model = new UserModel {
                Name = name,
                Surname = surname,
                Id_Group = group
            }; 
            if (Gateway.InsertEmptyMember(model)) {
                return new User(model);
            }
            return null;
        }

        private bool IsChanged { get; set; }

        private static UserGateway gateway;
        private static UserGateway Gateway => gateway ?? (gateway = new UserGateway());


        private async Task<Collection<Badge>> BudgesLazyLoad() {
            if (budges == null) {
                budges = await Zal.Badges.GetAcquired(this) as Collection<Badge>;
            }
            return budges;
        }

        public User(UserModel model) {
            this.model = model;
        }

        public static async Task<User> Login(string email, string password) {
            User user = new User(await Gateway.Login(email, password));
            return user;
        }

        //public static User Empty() {
        //    return new User("", "", "", "");
        //}


        /*public bool Has(int key, int value) {
            switch (key) {
                case CONST.USER.DRUZINA: return model.Id_druzina == value;
                default: return false;
            }
        }

        public bool Has(int key, String value) {
            switch (key) {
                case CONST.USER.EMAIL: return value.Equals(model.Email);
                case CONST.USER.JMENO: return value.Equals(model.ShortName);
                default: return false;
            }
        }*/

        public static async Task<IEnumerable<User>> GetAllMembers() {
            IEnumerable<UserModel> rawModels = await Gateway.GetAllMembersAsync();
            IEnumerable<User> users = rawModels.Select(model => new User(model));
            return users;
        }

        public static bool CheckForChanges(int userCount, DateTime lastCheck) {
            return Gateway.CheckForChanges(userCount, lastCheck);
        }

        public static async Task<User> GetAsync(int id) {
            return new User(await Gateway.GetAsync(id));
        }

        public static bool Contains(string email) {
            return Gateway.Contains(email);
        }

        public static async Task<User> RegisterNewAsync(string email, string name, string surname, string phone, string password) {
            var requestModel = new RegistrationRequestModel {
                Name = name,
                Surname = surname, 
                Phone = phone,
                Email = email,
                Password = password
            };
            if (await Gateway.RegisterAsync(requestModel)) {
                return new User(new UserModel(requestModel));
            }
            return null;
        }

        internal static async Task<IEnumerable<User>> GetAsync(IEnumerable<int> ids) {
            IEnumerable<UserModel> rawModels = await Gateway.GetAsync(ids);
            IEnumerable<User> users = rawModels.Select(model => new User(model));
            return users;
        }

        public void BecomeMember(DateTime dateOfBirthDay, int group, string prezdivka = null) {
            //if (model.Role == ZAL.MEMBERSHIP.NECLEN) {
                model.BirthDate = dateOfBirthDay;
            if (prezdivka != null) model.NickName = prezdivka;
                //model.Role = ZAL.MEMBERSHIP.CLEN;
                model.Id_Group = group;
                Gateway.BecomeMember(model);
            //}
            //else {
            //    throw new Exception("user already is a member");
            //}
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

        /*public void ConfirmChanges() {
            if (IsChanged) {
                Gateway.Update(new UzivatelTable(model, model));
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
            if (model.Zaplatil_prispevek != value) {
                model.Zaplatil_prispevek = value;
                Gateway.Update(model.Email, value);
            }
        }

        private void ChangeName(string value) {
            if (model.Jmeno != value) {
                model.Jmeno = value;
                SetShortName();
                IsChanged = true;
            }
        }

        private void ChangeSurname(string value) {
            if (model.Prijmeni != value) {
                model.Prijmeni = value;
                SetShortName();
                IsChanged = true;
            }
        }

        private void ChangeNick(string value) {
            if (model.Prezdivka != value) {
                model.Prezdivka = value;
                SetShortName();
                IsChanged = true;
            }
        }

        private void ChangePhone(string value) {
            if (model.Kontakt != value) {
                model.Kontakt = value;
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
            if (model.Role != value) {
                if (value == ZAL.MEMBERSHIP.CLEN) {
                    if (model.Datum_narozeni == DateTime.MinValue) {
                        return;
                    }
                }
                model.Role = value;
                IsChanged = true;
            }
        }

        private void ChangeGroup(int druzina) {
            if (druzina != model.Id_druzina) {
                if (druzina == ZAL.GROUP.LISKY) {
                    model.Id_druzina = druzina;
                    model.Hodnost = ZAL.RANK.LISKA;
                }
                else if (druzina == ZAL.GROUP.TROSKY) {
                    model.Id_druzina = druzina;
                    if (model.Hodnost < ZAL.RANK.VEDOUCI) {
                        model.Hodnost = ZAL.RANK.VEDOUCI;
                    }
                }
                else {
                    model.Id_druzina = druzina;
                    if (model.Hodnost < ZAL.RANK.NOVACEK || model.Hodnost > ZAL.RANK.RADCE) {
                        model.Hodnost = ZAL.RANK.NOVACEK;
                    }
                }
                IsChanged = true;
            }
        }

        private void ChangeRank(int hodnost) {
            if (hodnost != model.Hodnost) {
                if (hodnost == ZAL.RANK.LISKA) {
                    model.Id_druzina = ZAL.GROUP.LISKY;
                    model.Hodnost = hodnost;
                }
                else if (hodnost >= ZAL.RANK.VEDOUCI) {
                    model.Id_druzina = ZAL.GROUP.TROSKY;
                    model.Hodnost = hodnost;
                }
                else {
                    model.Hodnost = hodnost;
                    if (model.Id_druzina == ZAL.GROUP.LISKY || model.Id_druzina == ZAL.GROUP.TROSKY) {
                        model.Id_druzina = ZAL.GROUP.BOBRI;
                    }
                }
                IsChanged = true;
            }
        }

        private void SetShortName() {
            if (model.Prezdivka != null) {
                model.ShortName = model.Prezdivka;
            }
            else {
                if (model.Prijmeni.Length == 0) {
                    model.ShortName = model.Jmeno;
                }
                else {
                    model.ShortName = model.Jmeno + model.Prijmeni[0] + '.';
                }
            }
        }

        public bool ChangePassword(string userEmail, string oldPass, string newPass) {
            return Gateway.UpdatePassword(userEmail, oldPass, newPass);
        }*/

        public async Task<bool> AddBudget(Badge budget) {
            if (await Gateway.InsertBadget(Email, budget.Id)) {
                budges.Add(budget);
                return true;
            }
            return false;
        }

        public static bool Exists(string email) {
            return Gateway.Contains(email);
        }

        public override string ToString() {
            return Nick;
        }

        public bool IsLeader() {
            return RankLevel >= ZAL.RANK.VEDOUCI;
        }
    }
}
