using ZalDomain.ActiveRecords;
using ZalDomain.consts;
using ZalDomain.tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZalDomain.ItemSets
{
    public class ActualitySet
    {
        private ActualityObservableSortedSet Data { get; set; }
        private DateTime LastCheck;

        public ActualitySet() {
            Data = new ActualityObservableSortedSet();
            LastCheck = ZAL.DATE_OF_ORIGIN;
        }

        public void AddNewArticle(string title, string text, int fromRank, int? forGroup = null) {
            Data.Add(Actuality.MakeArticle(title, text, fromRank, forGroup));
        }

        public void AddNewArticle(User author, string title, string text, int fromRank, int? forGroup = null) {
            Data.Add(Actuality.MakeArticle(author, title, text, fromRank, forGroup));
        }

        public void AddNewInfo(ActionEvent action, string text, decimal gpsLon, decimal gpsLat, int price) {//?
            Data.Add(Actuality.MakeInfo(text, gpsLon, gpsLat, action.Id, price));
        }

        public void AddNewRecord(ActionEvent action, string text) {//akce.AddRecord(text) ?
            Data.Add(Actuality.MakeRecord(text, action.Id));
        }

        public bool Remove(Actuality item) {
            if (Zal.Me.IsLeader()) { }
            Data.Remove(item);
            return Actuality.Delete(item);
        }

        internal void Synchronize() {
            DateTime tmp = DateTime.Now;
            CheckForChanges();
            LastCheck = tmp;
        }

        internal void ReSynchronize() {
            LastCheck = ZAL.DATE_OF_ORIGIN;
            Synchronize();
        }

        private void CheckForChanges() {
            string changes = Actuality.CheckForChanges(Zal.Me, LastCheck);
            if (changes.Equals(CONST.CHANGES.MAJOR)) {
                Data.Clear();
                Data.AddAll(Actuality.GetAllFor(Zal.Me));
            }
            else if (changes.Equals(CONST.CHANGES.MINOR)) {
                List<int> changedItems = Actuality.GetChanged(Zal.Me, LastCheck);
                foreach (int id in changedItems) {
                    if (id < 0) {
                        Data.RemoveById(-id);
                    }
                    else {
                        Data.RemoveById(id);
                        Data.Add(Actuality.Get(id));
                    }
                }
            }
        }

        internal InfoAction GetInfoForAction(int idAction) {
            foreach (Actuality a in Data) {
                if(a.Type == "info") {
                    if(idAction == ((InfoAction)a.Item).Data.Id_akce) {
                        return (InfoAction)a.Item;
                    }
                }              
            }
            return null;
        }

        internal RecordAction GetZapisForAction(int idAction) {
            foreach (Actuality a in Data) {
                if (a.Type == "zapis") {
                    if (idAction == ((RecordAction)a.Item).Data.Id_akce) {
                        return (RecordAction)a.Item;
                    }
                }
            }
            return null;
        }

        public IActualityItem GetById(int id) {
            foreach (Actuality a in Data) {
                if (a.Id == id) {
                    return a.ItemLazyLoad();
                }
            }
            return Data.First().ItemLazyLoad();//?
        }

        public IActualityItem Get(Actuality a) {
            return a.ItemLazyLoad();
        }

        public IEnumerable<Actuality> GetAll() {
            Synchronize();
            return Data;
        }

        internal XElement GetXml(string elementName) {
            XElement element = new XElement(elementName);
            foreach (Actuality a in Data) {
                element.Add(a.GetXml("Actuality"));
            }
            return element;
        }

        internal void LoadFromXml(XElement element) {
            IEnumerable<XElement> data = element.Elements("Actuality");
            foreach (XElement el in data) {
                Actuality actuality = Actuality.LoadFromXml(el);
                if (!Data.Contains(actuality)){ //contains vs containsById
                    Data.Add(Actuality.LoadFromXml(el));
                }
            }
        }
    }
}