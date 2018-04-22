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
using ZalDomain.Models;

namespace ZalDomain.ItemSets
{
    public class ActionSet {

        public ActionObservableSortedSet UpcomingActionEvents { get; set; }
        private Dictionary<int, ActionObservableSortedSet> ActionEventsDict { get; set; } 
        private int ActiveYear;
        private int CurrentYear;
        private bool IsCurrentYear => ActiveYear == CurrentYear;

        public ActionSet() {
            UpcomingActionEvents = new ActionObservableSortedSet();
            ActionEventsDict = new Dictionary<int, ActionObservableSortedSet>();
            CurrentYear = ActiveYear = DateTime.Today.Year;
        }

        public async Task<ActionObservableSortedSet> GetActionEventsByYearAsync(int year) {
            ActiveYear = year;
            await SynchronizeDataInAsync(year);
            return ActionEventsDict[year];
        }

        //když vytvořím 2 stejné akce v offline režimu, tak se navzájem vyruší    

        public async Task ReSynchronizeAsync() {
            UpcomingActionEvents.Clear();
            ActionEventsDict.Clear();
            await SynchronizeAsync();
        }

        public async Task SynchronizeAsync() {
            await SynchronizeDataInAsync(CurrentYear);
            if (!IsCurrentYear) {
                await SynchronizeDataInAsync(ActiveYear);
            }
        }

        private async Task SynchronizeDataInAsync(int year) {
            AddToDictionaryIfNeeded(year);
            if (ActionEventsDict[year].HasToBeReloaded) {
                await GetAllByYearAsync(year);
            }
            else {
                await GetChangedByYearAsync(year);
            }
        }

        private async Task GetAllByYearAsync(int year) {
            var respond = await ActionEvent.GetAllByYear(Zal.Session.UserRank, year);
            ActionEventsDict[year] = new ActionObservableSortedSet(respond.ActiveRecords) {
                LastSynchronization = respond.Timestamp
            };
        }

        private async Task GetChangedByYearAsync(int year) {
            var respond = await ActionEvent.GetChangedAsync(Zal.Session.UserRank, ActionEventsDict[year].LastSynchronization, year, ActionEventsDict[year].Count);
            if (respond.IsHardChanged) {
                ActionEventsDict[year] = new ActionObservableSortedSet(respond.Changed);
            }
            else if (respond.IsChanged) {
                RefreshDataIn(year, respond);
            }
            ActionEventsDict[year].LastSynchronization = respond.Timestamp;
            if (respond.HasAnyChanges && IsCurrentYear) {
                RefreshUpcomingActions();
            }
        }

        private void RefreshDataIn(int year, ChangedActiveRecords<ActionEvent> changes) {
            ActionEventsDict[year].RemoveByIds(changes.Deleted);
            ActionEventsDict[year].AddOrUpdateAll(changes.Changed);
        }

        private void RefreshUpcomingActions() {
            var tmp = ActionEventsDict[CurrentYear].Where(action => action.DateTo >= DateTime.Now);
            UpcomingActionEvents = new ActionObservableSortedSet(tmp) {
                LastSynchronization = ActionEventsDict[CurrentYear].LastSynchronization
            };
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
            AddToDictionaryIfNeeded(item.DateFrom.Year);
            ActionEventsDict[item.DateFrom.Year].Add(item);
            if (item.DateTo >= DateTime.Now) {
                UpcomingActionEvents.Add(item);
            }
        }

        private void PlaceIntoRelevantCollections(IEnumerable<ActionEvent> items, int year) {
            AddToDictionaryIfNeeded(year);
            ActionEventsDict[year] = ActionEventsDict[year].Union(items) as ActionObservableSortedSet;
            UpcomingActionEvents = UpcomingActionEvents.Union(items.Where(action => action.DateTo >= DateTime.Now)) as ActionObservableSortedSet;
        }

        private void AddToDictionaryIfNeeded(int year) {
            if (!ActionEventsDict.ContainsKey(year)) {
                ActionEventsDict.Add(year, new ActionObservableSortedSet());
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

        internal JToken GetJson() {
            JArray jArray = new JArray();
            if (ActionEventsDict.ContainsKey(CurrentYear)) {
                foreach (ActionEvent a in ActionEventsDict[CurrentYear]) {
                    jArray.Add(a.GetJson());
                }
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
                ActionEventsDict[akce.DateFrom.Year].Remove(akce);
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
