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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ZalDomain.ItemSets
{
    public class ActionSet {

        public ActionObservableSortedSet UpcomingActionEvents { get; set; }
        private Dictionary<int, ActionObservableSortedSet> ActionEvents { get; set; } 
        private DateTime LastCheck;
        private int ActiveYear;
        private int CurrentYear;

        public ActionSet() {
            UpcomingActionEvents = new ActionObservableSortedSet();
            ActionEvents = new Dictionary<int, ActionObservableSortedSet>();
            LastCheck = ZAL.DATE_OF_ORIGIN;
            CurrentYear = ActiveYear = DateTime.Today.Year;
        }

        public async Task<ActionObservableSortedSet> GetActionEventsByYearAsync(int year) {
            ActiveYear = year;
            await SynchronizeDataInAsync(year);
            return ActionEvents[year];
        }

        //když vytvořím 2 stejné akce v offline režimu, tak se navzájem vyruší    

        public async Task ReSynchronizeAsync() {
            UpcomingActionEvents.Clear();
            ActionEvents.Clear();
            LastCheck = ZAL.DATE_OF_ORIGIN;
            await SynchronizeAsync();
        }

        public async Task SynchronizeAsync() {
            DateTime tmp = DateTime.Now;
            await SynchronizeDataInAsync(ActiveYear);
            if (CurrentYear != ActiveYear) {
                await SynchronizeDataInAsync(CurrentYear);
            }
            RefreshUpcomingActions();    
            LastCheck = tmp;//čas serveru nemusí být stejný (nepoužívat místní)
        }

        private async Task SynchronizeDataInAsync(int year) {
            if (ActionEvents.ContainsKey(year)) {
                await ActualizeDataInAsync(year);
            }
            else {
                ActionEvents.Add(year, new ActionObservableSortedSet());
                await GetAllByYearAsync(year);
            }
        }

        private async Task ActualizeDataInAsync(int year) {
            var respond = await ActionEvent.GetChangedAsync(Zal.Session.UserRank, LastCheck, year, ActionEvents[year].Count);
            if (respond.IsHardChanged) {
                ActionEvents[year] = new ActionObservableSortedSet(respond.Changed);
            }
            else if (respond.IsChanged) {
                RefreshDataIn(year, respond);
            }
        }

        private async Task GetAllByYearAsync(int year) {
            var respond = await ActionEvent.GetAllByYear(Zal.Session.UserRank, year);
            ActionEvents[year] = new ActionObservableSortedSet(respond);
        }

        private void RefreshUpcomingActions() {
            var tmp = ActionEvents[CurrentYear].Where(action => action.DateTo >= DateTime.Now);
            UpcomingActionEvents = new ActionObservableSortedSet(tmp);
        }

        private void RefreshDataIn(int year, ChangedActiveRecords<ActionEvent> changes) {
            var tmp = ActionEvents[year].Where(action => changes.Deleted.All(id => id != action.Id));
            tmp.Union(changes.Changed, new ActiveRecordEqualityComparer());
            ActionEvents[year] = new ActionObservableSortedSet(tmp);
        }

        public async Task<bool> AddNewActionAsync(string name, string type, DateTime dateFrom, DateTime dateTo, int fromRank, bool isOfficial) {
            bool isAdded = false;
            ActionEvent item = await ActionEvent.AddAsync(name, type, dateFrom, dateTo, fromRank, isOfficial);
            if (item != null) {
                PlaceIntoRelevantCollections(item);
                isAdded = true;
            }
            return isAdded;
        }

        private void PlaceIntoRelevantCollections(ActionEvent item) {
            if (!ActionEvents.ContainsKey(item.DateFrom.Year)) {
                ActionEvents.Add(item.DateFrom.Year, new ActionObservableSortedSet());
            }
            ActionEvents[item.DateFrom.Year].Add(item);
            if (item.DateTo >= DateTime.Now) {
                UpcomingActionEvents.Add(item);
            }
        }

        private void PlaceIntoRelevantCollections(IEnumerable<ActionEvent> items, int year) {
            if (!ActionEvents.ContainsKey(year)) {
                ActionEvents.Add(year, new ActionObservableSortedSet());
            }
            ActionEvents[year] = ActionEvents[year].Union(items) as ActionObservableSortedSet;
            UpcomingActionEvents = UpcomingActionEvents.Union(items.Where(action => action.DateTo >= DateTime.Now)) as ActionObservableSortedSet;
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

        internal JToken GetJson() {
            JArray jArray = new JArray();               
            foreach (ActionEvent a in ActionEvents[CurrentYear]) {
                jArray.Add(a.GetJson());
            }
            return jArray;
        }

        internal void LoadFrom(JToken json) {
            var actions = json.Select(x => ActionEvent.LoadFrom(x));
            if (actions.Count() >= 1) {
                int year = actions.First().DateFrom.Year;
                PlaceIntoRelevantCollections(actions, year);
            }
        }

        public async Task<bool> DeleteAsync(ActionEvent akce) {
            bool isDeleted = await akce.DeleteAsync();
            if (isDeleted) {
                ActionEvents[akce.DateFrom.Year].Remove(akce);
                UpcomingActionEvents.Remove(akce);
            }
            return isDeleted;
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
