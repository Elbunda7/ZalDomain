using ZalDomain.ActiveRecords;
using ZalDomain.consts;
using ZalDomain.tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZalApiGateway.Models;

namespace ZalDomain.ItemSets
{
    public class ActionSet {

        public ActionObservableSortedSet UpcomingActionEvents { get; set; }
        private Dictionary<int, ActionObservableSortedSet> ActionEvents { get; set; } 
        private DateTime LastCheck;
        private int CurrentYear;

        public ActionSet() {
            UpcomingActionEvents = new ActionObservableSortedSet();
            ActionEvents = new Dictionary<int, ActionObservableSortedSet>();
            LastCheck = ZAL.DATE_OF_ORIGIN;
            CurrentYear = DateTime.Today.Year;
        }

        public ActionObservableSortedSet GetActionEventsByYear(int year) {
            CurrentYear = year;
            if (ActionEvents.ContainsKey(year)) {
                Synchronize();
                return ActionEvents[year];
            }
            else {
                ActionEvents.Add(year, new ActionObservableSortedSet());
                GetAllByYear(year);
                return ActionEvents[year];
            }
        }

        private async void GetAllByYear(int year) {
            ActionEvents[year] = await ActionEvent.GetAllByYear(Zal.Session.UserRank, year) as ActionObservableSortedSet;
        }    

        //private ActionObservableSortedSet Data; //když vytvořím 2 stejné akce v offline režimu, tak se navzájem vyruší
        //private List<int> AvilableDataCategory;       

        public void Synchronize() {
            DateTime tmp = DateTime.Now;
            SynchronizeChanges();
            LastCheck = tmp;
        }

        public void ReSynchronize() {
            UpcomingActionEvents.Clear();
            ActionEvents.Clear();
            LastCheck = ZAL.DATE_OF_ORIGIN;
            Synchronize();
        }

        private async void SynchronizeChanges() {
            ChangesRequestModel requestModel = new ChangesRequestModel {
                    Rank = Zal.Session.UserRank,
                    LastCheck = LastCheck,
                    Year = CurrentYear,
                    Count = ActionEvents.Count
                };
            var respond = await ActionEvent.GetChangedAsync(requestModel);
            ActualizeDataWith(ActionEvents[CurrentYear], respond);
            if (DateTime.Today.Year != CurrentYear) {
                requestModel.Year = DateTime.Today.Year;
                respond = await ActionEvent.GetChangedAsync(requestModel);
                ActualizeDataWith(ActionEvents[DateTime.Today.Year], respond);
            }
            UpcomingActionEvents = GetUpcoming();
        }

        private void ActualizeDataWith(ActionObservableSortedSet data, ChangedActiveRecords<ActionEvent> changes) {
            IEnumerable<ActionEvent> tmp;
            tmp = data.Where(action => changes.Deleted.All(id => id != action.Id));
            tmp.Union(changes.Changed, new ActiveRecordEqualityComparer());
            data = tmp as ActionObservableSortedSet;
        }

        private ActionObservableSortedSet GetUpcoming() {
            var tmp = ActionEvents[DateTime.Today.Year].Where(action => action.DateTo >= DateTime.Now);
            return tmp as ActionObservableSortedSet;
        }

        public async void InsertNewAction(string name, string type, DateTime dateFrom, DateTime dateTo, int fromRank, bool isOfficial) {
            if (!ActionEvents.ContainsKey(dateFrom.Year)) {
                ActionEvents.Add(dateFrom.Year, new ActionObservableSortedSet());
            }
            ActionEvent item = await ActionEvent.AddAsync(name, type, dateFrom, dateTo, fromRank, isOfficial);
            ActionEvents[dateFrom.Year].Add(item);
            if (dateTo >= DateTime.Now) {
                UpcomingActionEvents.Add(item);
            }
        }

        /*public ActionEvent GetById(int value) {
            foreach (ActionEvent a in Data) {
                if (a.Has(CONST.AKCE.ID, value)) {
                    return a;
                }
            }
            return null;
        }

        public Collection<ActionEvent> GetBy(int key, int value) {
            Synchronize();
            Collection<ActionEvent> tmp = new Collection<ActionEvent>();
            foreach (ActionEvent a in Data) {
                if (a.Has(key, value)) {
                    tmp.Add(a);
                }
            }
            return tmp;
        }

        public Collection<ActionEvent> GetBy(int key, String value) {
            Synchronize();
            Collection<ActionEvent> tmp = new Collection<ActionEvent>();
            foreach (ActionEvent a in Data) {
                if (a.Has(key, value)) {
                    tmp.Add(a);
                }
            }
            return tmp;
        }

        public Collection<ActionEvent> GetBy(int key, bool value) {
            Collection<ActionEvent> tmp = new Collection<ActionEvent>();
            foreach (ActionEvent a in Data) {
                if (a.Has(key, value)) {
                    tmp.Add(a);
                }
            }
            return tmp;
        }

        public Collection<ActionEvent> GetBy(int key, DateTime value) {
            Collection<ActionEvent> tmp = new Collection<ActionEvent>();
            foreach (ActionEvent a in Data) {
                if (a.Has(key, value)) {
                    tmp.Add(a);
                }
            }
            return tmp;
        }*/

        internal XElement GetXml(string elementName) {
            /*XElement element = new XElement(elementName);
            foreach (ActionEvent a in Data) {
                element.Add(a.GetXml("Action"));
            }
            return element;*/
            throw new NotImplementedException();
        }

        internal void LoadFromXml(XElement element) {
            /*IEnumerable<XElement> data = element.Elements("Action");
            foreach (XElement el in data) {
                Data.Add(ActionEvent.LoadFromXml(el));
            }
            */
           throw new NotImplementedException();
        }

        public async void Delete(ActionEvent akce) {
            if (await akce.DeleteAsync()) {
                ActionEvents[akce.DateFrom.Year].Remove(akce);
                UpcomingActionEvents.Remove(akce);
            }
        }

        /*internal Collection<Akce> getAll() {
            return SqlAkce.getAll();
        }*/

        //internal void getUpcoming() {
        //    SqlAkce.GetUpcoming();
        //}

        /*internal Akce get(int id) {
            return SqlAkce.get(id);
        }*/
    }
}
