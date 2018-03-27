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

namespace ZalDomain.ItemSets
{
    public class ActionSet {

        public ActionObservableSortedSet UpcomingActionEvents { get; set; }
        private Dictionary<int, ActionObservableSortedSet> ActionEvents { get; set; } 
        private DateTime LastCheck;
        private int ActiveYear;

        public ActionSet() {
            UpcomingActionEvents = new ActionObservableSortedSet();
            ActionEvents = new Dictionary<int, ActionObservableSortedSet>();
            LastCheck = ZAL.DATE_OF_ORIGIN;
            ActiveYear = DateTime.Today.Year;
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
            if (DateTime.Today.Year != ActiveYear) {
                await SynchronizeDataInAsync(DateTime.Today.Year);
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
            var tmp = ActionEvents[DateTime.Today.Year].Where(action => action.DateTo >= DateTime.Now);
            UpcomingActionEvents = new ActionObservableSortedSet(tmp);
        }

        private void RefreshDataIn(int year, ChangedActiveRecords<ActionEvent> changes) {
            var tmp = ActionEvents[year].Where(action => changes.Deleted.All(id => id != action.Id));
            tmp.Union(changes.Changed, new ActiveRecordEqualityComparer());
            ActionEvents[year] = new ActionObservableSortedSet(tmp);
        }

        public async Task<bool> AddNewActionAsync(string name, string type, DateTime dateFrom, DateTime dateTo, int fromRank, bool isOfficial) {
            if (!ActionEvents.ContainsKey(dateFrom.Year)) {
                ActionEvents.Add(dateFrom.Year, new ActionObservableSortedSet());
            }
            bool isAdded = false;
            ActionEvent item = await ActionEvent.AddAsync(name, type, dateFrom, dateTo, fromRank, isOfficial);
            if (item != null) {
                ActionEvents[dateFrom.Year].Add(item);
                if (dateTo >= DateTime.Now) {
                    UpcomingActionEvents.Add(item);
                }
                isAdded = true;
            }
            return isAdded;
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
