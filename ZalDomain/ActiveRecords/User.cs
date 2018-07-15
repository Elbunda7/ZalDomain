﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ZalApiGateway;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.consts;
using Newtonsoft.Json;
using ZalDomain.tools;
using ZalDomain.Models;
using ZalApiGateway.Consts;

namespace ZalDomain.ActiveRecords
{
    public class User : IActiveRecord {

        private UserModel Model;

        private Collection<Badge> budges;

        public int Id => Model.Id;
        public string Email => Model.Email;
        public string Nick => Model.NickName;
        public string Name => Model.Name;
        public string Surname => Model.Surname;
        public string Phone => Model.Phone;
        public ZAL.Rank Rank => (ZAL.Rank)Model.Id_Rank;
        public ZAL.Group Group => (ZAL.Group)Model.Id_Group;
        public string RankAsString => ZAL.RANK_NAME[Model.Id_Rank];
        public string GroupAsString => ZAL.GROUP_NAME_SINGULAR[Model.Id_Group];
        public DateTime? DateOfBirth => Model.BirthDate;
        //public string Role { get { return model.Role; } }
        //public int Points { get { return model.Body; } }//todo

        //public bool PaidForMembership { get { return model.Zaplatil_prispevek; } }
        //public Collection<Badge> Budges { get { return BudgesLazyLoad(); } private set { budgets = value; } }

        private static UserGateway gateway;
        private static UserGateway Gateway => gateway ?? (gateway = new UserGateway());

        private UnitOfWork<UserUpdateModel> unitOfWork;

        public UnitOfWork<UserUpdateModel> UnitOfWork => unitOfWork ?? (unitOfWork = new UnitOfWork<UserUpdateModel>(Model, OnUpdateCommited));

        private Task<bool> OnUpdateCommited() {
            return Gateway.UpdateAsync(Model, Zal.Session.Token);
        }

        internal static async Task<ChangedActiveRecords<User, UserModel>> GetChanged(UserFilterModel filter, DateTime lastCheck, int count) {
            var requestModel = new UserChangesRequestModel {
                Groups = (int)filter.Groups,
                Ranks = (int)filter.Ranks,
                Roles = (int)filter.Roles,
                Mode = Const.FilterMode.AND,
                LastCheck = lastCheck,
                Count = count
            };
            var respond = await Gateway.GetAllChangedAsync(requestModel, Zal.Session.Token);
            var items = respond.Changed.Select(x => new User(x));
            return new ChangedActiveRecords<User, UserModel>(respond, items);
        }

        internal bool Meets(UserFilterModel filter) {
            return filter.CanContains(Group, Rank, (ZAL.UserRole)7);//todo role
        }

        internal static async Task<User> AddNewUser(string name, string surname, int group, string nickname = null, string phone = null, string email = null, DateTime? birthDate = null) {
            UserModel model = new UserModel {
                NickName = nickname ?? $"{name} {surname[0]}.",
                Name = name,
                Surname = surname,
                Id_Group = group,
                BirthDate = birthDate,
                Email = email,
                Phone = phone,
            }; 
            if (await Gateway.AddAsync(model, Zal.Session.Token)) {
                return new User(model);
            }
            return null;
        }

        private async Task<Collection<Badge>> BudgesLazyLoad() {
            if (budges == null) {
                budges = await Zal.Badges.GetAcquired(this) as Collection<Badge>;
            }
            return budges;
        }

        public User(UserModel model) {
            this.Model = model;
        }

        //public static User Empty() {
        //    return new User("", "", "", "");
        //}

        public static async Task<AllActiveRecords<User>> GetAll(UserFilterModel filter, bool isAndMode) {
            var requestModel = new UserRequestModel() {
                Groups = (int)filter.Groups,
                Ranks = (int)filter.Ranks,
                Roles = (int)filter.Roles,//todo not ready jet
                Mode = isAndMode ? Const.FilterMode.AND : Const.FilterMode.OR,
            };
            var respond = await Gateway.GetAllAsync(requestModel);
            return new AllActiveRecords<User>() {
                Timestamp = respond.Timestamp,
                ActiveRecords = respond.GetItems().Select(model => new User(model)),
            };
        }

        public static async Task<User> GetAsync(int id) {
            return new User(await Gateway.GetAsync(id));
        }

        internal static async Task<IEnumerable<User>> GetAsync(IEnumerable<int> ids) {
            IEnumerable<UserModel> rawModels = await Gateway.GetAsync(ids);
            IEnumerable<User> users = rawModels.Select(model => new User(model));
            return users;
        }

        public void BecomeMember(DateTime dateOfBirthDay, int group, string prezdivka = null) {
            //if (model.Role == ZAL.MEMBERSHIP.NECLEN) {
                Model.BirthDate = dateOfBirthDay;
            if (prezdivka != null) Model.NickName = prezdivka;
                //model.Role = ZAL.MEMBERSHIP.CLEN;
                Model.Id_Group = group;
                Gateway.BecomeMember(Model);
            //}
            //else {
            //    throw new Exception("user already is a member");
            //}
        }

        /*rivate void ChangeIsPaid(bool value) {
            if (model.Zaplatil_prispevek != value) {
                model.Zaplatil_prispevek = value;
                Gateway.Update(model.Email, value);
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

        public override string ToString() {
            return Nick;
        }

        [Obsolete]
        public bool IsLeader() {
            return Rank >= ZAL.Rank.Vedouci;
        }

        internal static User LoadFrom(JToken jToken) {
            UserModel model = jToken.ToObject<UserModel>();
            return new User(model);
        }

        internal JToken GetModelJson() {
            return JToken.FromObject(Model);
        }
    }
}
