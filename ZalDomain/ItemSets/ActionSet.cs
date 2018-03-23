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

        private ActionObservableSortedSet Data; //když vytvořím 2 stejné akce v offline režimu, tak se navzájem vyruší
        private DateTime LastCheck;

        public ActionSet() {
            Data = new ActionObservableSortedSet();
            LastCheck = ZAL.DATE_OF_ORIGIN;
        }

        public void Synchronize() {
            DateTime tmp = DateTime.Now;
            CheckForChanges();
            LastCheck = tmp;
        }

        public void ReSynchronize() {
            Data.Clear();
            LastCheck = ZAL.DATE_OF_ORIGIN;
            Synchronize();
        }

        private async void CheckForChanges() {
            Data.Clear();
            Data.AddAll(await ActionEvent.GetUpcoming(Zal.Session.CurrentUser));
            /*string changes = ActionEvent.Synchronize(Zal.Me, LastCheck);//nepozná když se záznam vymaže natvrdo
            if (changes.Equals(CONST.CHANGES.MAJOR)) {
                Data.Clear();
                Data.AddAll(ActionEvent.GetUpcoming(Zal.Me));
            }
            else if (changes.Equals(CONST.CHANGES.MINOR)) {
                List<int> changedItems = ActionEvent.SynchronizeAll(Zal.Me, LastCheck);
                foreach (int id in changedItems) {
                    if (id < 0) {
                        Data.RemoveById(-id);
                    }
                    else {
                        Data.RemoveById(id);
                        Data.Add(ActionEvent.Get(id));
                    }
                }
            }*/
        }

        public void InsertNewAction(string name, string type, DateTime start, DateTime end, int fromRank, bool isOfficial) {
            Data.Add(new ActionEvent(name, type, start, end, fromRank, isOfficial));
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
            XElement element = new XElement(elementName);
            foreach (ActionEvent a in Data) {
                element.Add(a.GetXml("Action"));
            }
            return element;
        }

        internal void LoadFromXml(XElement element) {
            IEnumerable<XElement> data = element.Elements("Action");
            foreach (XElement el in data) {
                Data.Add(ActionEvent.LoadFromXml(el));
            }
        }

        public ICollection<ActionEvent> GetAll() {
            Synchronize();
            return Data;
        }

        public async void Delete(ActionEvent akce) {
            if (await akce.DeleteAsync()) {
                Data.Remove(akce);
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
