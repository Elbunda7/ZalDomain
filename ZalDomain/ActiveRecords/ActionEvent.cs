using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using ZalDomain.consts;
using System.Xml.Linq;
using ZalDomain.tools;
using ZalApiGateway.Models;
using ZalApiGateway;
using System.Threading.Tasks;
using ZalApiGateway.Models.ApiCommunicationModels;
using Newtonsoft.Json.Linq;

namespace ZalDomain.ActiveRecords
{
    public class ActionEvent : IActiveRecord
    {
        private ActionModel Model;
        //private AkceTable 
        private Collection<User> garants;
        private Collection<User> participants;
        private Article Report { get; set; }
        private Article Info { get; set; }
        
        public int Id => Model.Id;
        public string Type => Model.EventType;
        public string Name => Model.Name;
        public int Days => GetDays();
        public int FromRank => Model.FromRank;
        public DateTime DateFrom => Model.Date_start;
        public DateTime DateTo => Model.Date_end;
        public Collection<User> Garants { get { return GarantsLazyLoad(); } set { SetGarants(value); } }
        //public Collection<User> Participants { get { return ParticipantsLazyLoad(); } private set { participants = value; } }
        //public int ParticipantsCount => Participants.Count; //Model.NumOfParticipants
        //public bool Je_oficialni { get; set; }
        //účastním se?

        //public int Price { get { return Data.Price; } }
        //public decimal GPS_lon { get { return Data.GPS_lon; } }
        //public decimal GPS_lat { get { return Data.GPS_lat; } }
        public bool HasInfo => Model.Id_Info.HasValue;
        public bool HasReport => Model.Id_Report.HasValue;

        private static ActionGateway gateway;
        private static ActionGateway Gateway => gateway ?? (gateway = new ActionGateway());

        private int GetDays() {
            TimeSpan ts = Model.Date_end - Model.Date_start;
            return (int)ts.TotalDays;
        }

        public async Task<Article> InfoLazyLoad() {
            if (Info == null && Model.Id_Info.HasValue) {
                Info = await Zal.Actualities.GetArticleAsync(Model.Id_Info.Value);
            }
            return Info;
        }

        public async Task<Article> ReportLazyLoad() {
            if (Report == null && Model.Id_Report.HasValue) {
                    Report = await Zal.Actualities.GetArticleAsync(Model.Id_Report.Value);
            }
            return Report;
        }

        internal static async Task<ChangedActiveRecords<ActionEvent>> GetChangedAsync(int userRank, DateTime lastCheck, int currentYear, int count) {
            var requestModel = new ChangesRequestModel {
                Rank = userRank,
                LastCheck = lastCheck,
                Year = currentYear,
                Count = count
            };
            var rawChanges = await Gateway.GetAllChangedAsync(requestModel);
            var changes = new ChangedActiveRecords<ActionEvent> {
                Deleted = rawChanges.Deleted,
                Changed = rawChanges.Changed.Select(model => new ActionEvent(model))
            };
            return changes;
        }

        public ActionEvent(ActionModel model) {
            Model = model;
        }

        public static async Task<ActionEvent> AddAsync(string name, string type, DateTime start, DateTime end, int fromRank, bool isOfficial = true) {
            ActionModel model = new ActionModel {
                Id = -1,
                Name = name,
                EventType = type,
                Date_start = start,
                Date_end = end,
                FromRank = fromRank,
                IsOfficial = isOfficial,
            };
            if (await Gateway.AddAsync(model)) {
                return new ActionEvent(model);
            }
            return null;
        }

        public Collection<User> GarantsLazyLoad() {
            /*if (garants == null) {
                if (Model.Email_vedouci != null) {
                    garants = Zal.Users.GetByEmail(Model.Email_vedouci);
                }
                else {
                    garants = User.Empty();
                }               
            }
            return garants;*/
            throw new NotImplementedException();
        }

        public async Task<bool> AddNewInfoAsync(string title, string text) {
            //token uživatele
            return await AddNewInfoAsync(Zal.Session.CurrentUser, title, text);
        }

        public async Task<bool> AddNewInfoAsync(User author, string title, string text) {
            //token uživatele
            Info = await Zal.Actualities.CreateNewArticle(author, title, text, FromRank);
            if (Info != null) {
                Model.Id_Info = Info.Id;
                return await Gateway.UpdateAsync(Model);
            }
            return false;
        }

        public async Task<bool> AddNewReportAsync(string title, string text) {
            //token uživatele
            return await AddNewReportAsync(Zal.Session.CurrentUser, title, text);
        }

        public async Task<bool> AddNewReportAsync(User author, string title, string text) {
            //token uživatele
            Report = await Zal.Actualities.CreateNewArticle(author, title, text, FromRank);
            if (Report != null) {
                Model.Id_Report = Report.Id;
                return await Gateway.UpdateAsync(Model);
            }
            return false;
        }

        public async Task<Collection<User>> ParticipantsLazyLoad() {
            if (participants == null) {//obnovit?
                List<int> list = await gateway.GetParticipatingUsersAsync(Model.Id);
                participants = await Zal.Users.Get(list) as Collection<User>;
            }
            return participants;
        }

        /*public bool Has(int key, int value) {
            switch (key) {
                case CONST.AKCE.ID: return Model.Id == value;
                case CONST.AKCE.POCET_DNI: return Model.Pocet_dni == value;
                case CONST.AKCE.OD_HODNOSTI: return Model.Od_hodnosti == value;
                default: return false;
            }
        }

        public bool Has(int key, String value) {
            switch (key) {
                case CONST.AKCE.JMENO: return value.Equals(Model.Jmeno);
                case CONST.AKCE.TYP: return value.Equals(Model.Typ);
                case CONST.AKCE.EMAIL_VEDOUCI: return value.Equals(Model.Email_vedouci);
                default: return false;
            }
        }

        public bool Has(int key, bool value) {
            switch (key) {
                case CONST.AKCE.JE_OFICIALNI: return Model.Je_oficialni == value;
                default: return false;
            }
        }

        public bool Has(int key, DateTime value) {
            switch (key) {
                case CONST.AKCE.DATUM: return Model.Datum_od.Date == value.Date;
                case CONST.AKCE.DATUM_DO: return Model.Datum_od.Date <= value.Date;
                case CONST.AKCE.DATUM_OD: return Model.Datum_od.Date >= value.Date;
                default: return false;
            }
        }*/

        public bool SetGarants(Collection<User> garants) {
            /*IntegrityCondition.UserIsLeader();
            if (Gateway.JoinLeaderToAction(garants.Email, Model.Id)) {
                Garants = garants;
                return true;
            }
            return false;*/
            throw new NotImplementedException();
        }

        public async Task<bool> AktualizeAsync(String name, String type, DateTime? start, DateTime? end, int? fromRank, bool? isOfficial) {
            UserPermision.HasRank(Zal.Session.CurrentUser, ZAL.RANK.VEDOUCI);
            if (name != null) Model.Name = name;
            if (type != null) Model.EventType = type;
            if (end != null) Model.Date_end = end.Value;
            if (start != null) Model.Date_start = start.Value;
            if (fromRank != null) Model.FromRank = fromRank.Value;
            if (isOfficial != null) Model.IsOfficial = isOfficial.Value;
            return await Gateway.UpdateAsync(Model);
        }

        public async Task<bool> Participate(bool isGoing) {
            //token uživatele
            return await ParticipateAsync(Zal.Session.CurrentUser, isGoing);
        }

        public async Task<bool> ParticipateAsync(User user, bool isGoing) {
            return await Gateway.JoinAsync(user.Id, Model.Id);
        }

        public override string ToString() {
            return Model.Name;
        }

        public async void Synchronize(DateTime lastCheck) {
            Model = await Gateway.GetChangedAsync(Id, lastCheck);
        }

        public async static Task<IEnumerable<ActionEvent>> GetAllByYear(int userRank, int year) {
            var requestModel = new ActionRequestModel {
                Rank = userRank,
                Year = year
            };
            var models = await Gateway.GetAllByYearAsync(requestModel);
            IEnumerable<ActionEvent> actions = models.Select(model => new ActionEvent(model));
            return actions;
        }

        public async static Task<ActionEvent> Get(int id) {
            return new ActionEvent(await Gateway.GetAsync(id));
        }

        public async Task<bool> DeleteAsync() {
            return await Gateway.DeleteAsync(Model.Id);
        }

        internal JToken GetJson() {
            return JObject.FromObject(Model);
        }

        internal static ActionEvent LoadFrom(JToken json) {
            var model = json.ToObject<ActionModel>();
            return new ActionEvent(model);
        }
    }
}
