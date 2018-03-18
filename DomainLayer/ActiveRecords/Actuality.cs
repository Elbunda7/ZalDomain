using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DAL.tableStructures;
using DAL.Gateway;
using System.Xml.Linq;
using ZalDomain.tools;
using ZalDomain.consts;

namespace ZalDomain.ActiveRecords
{
    public class Actuality : IActiveRecord {

        private AktualityTable Data;
        private string type;
        private IActualityItem item;

        internal int Id { get { return Data.Id; } }
        internal DateTime DateOfCreation { get { return Data.DateOfCreation; } }

        public string Title { get { return Data.Title; } }
        public string ShortText { get { return Data.ShortText; } }
        public string Type { get { return GetItemType(); } }        //zrušit 3x null-ids, zařídit typ+id
        public IActualityItem Item { get { return ItemLazyLoad(); } private set { item = value; } }

        private static AktualityGateway gateway;
        private static AktualityGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = AktualityGateway.GetInstance();
                }
                return gateway;
            }
        }

        public Actuality() {
        }

        public static Actuality MakeArticle(string title, string Text, int fromRank, int? forGroup = null) {
            //IntegrityCondition.UserHasSecondRank();
            return MakeArticle(Zal.Me, title, Text, fromRank, forGroup);
        }

        public static Actuality MakeArticle(User author, string title, string Text, int fromRank, int? forGroup = null) {
            Article clanek = new Article(author, title, Text, fromRank, forGroup);
            return new Actuality(clanek);
        }

        public static Actuality MakeInfo(string text, decimal gpsLon, decimal gpsLat, int idAkce, int price) {
            //IntegrityCondition.UserIsLeader();
            InfoAction info = new InfoAction(text, gpsLon, gpsLat, idAkce, price);
            return new Actuality(info);
        }

        public static Actuality MakeRecord(string text, int idAkce) {
            //IntegrityCondition.UserHasSecondRank();
            RecordAction zapis = new RecordAction(text, idAkce);
            return new Actuality(zapis);
        }

        private Actuality(Article item) {
            Gateway.Insert(item.Data);
            Item = item;
            Data = new AktualityTable {
                Id = item.Data.Id_aktuality,
                Id_clanek = item.Data.Id,
                DateOfCreation = DateTime.Now,
                Title = item.Title,
                ShortText = item.GetShortText(0)
            };
        }

        private Actuality(InfoAction item) {
            Gateway.Insert(item.Data);
            this.item = item;
            Data = new AktualityTable {
                Id = item.Data.Id_aktuality,
                Id_info = item.Data.Id_akce,
                DateOfCreation = DateTime.Now,
                Title = item.Title,
                ShortText = item.GetShortText(0)
            };
        }

        internal Actuality(RecordAction item) {
            Gateway.Insert(item.Data);
            this.item = item;
            Data = new AktualityTable {
                Id = item.Data.Id_aktuality,
                Id_zapis = item.Data.Id_akce,
                DateOfCreation = DateTime.Now,
                Title = item.Title,
                ShortText = item.GetShortText(0)
            };
        }

        public Actuality(AktualityTable data) {
            Data = data;
        }

        public IActualityItem ItemLazyLoad() {
            if (item == null) {
                LoadFromDatabase();
            }
            return item;
        }

        private string GetItemType() {
            if (type == null) {
                if (Data.Id_info != null) {
                    type = ZAL.ACTUALITY_TYPE.INFO;
                }
                else if (Data.Id_zapis != null) {
                    type = ZAL.ACTUALITY_TYPE.RECORD;
                }
                else if (Data.Id_clanek != null) {
                    type = ZAL.ACTUALITY_TYPE.ARTICLE;
                }
                else {
                    throw new Exception("wrong actuality type");
                }
            }
            return type;
        }

        private void LoadFromDatabase() {
            switch (Type) {
                case ZAL.ACTUALITY_TYPE.INFO:
                    item = new InfoAction(Gateway.SelectInfo((int)Data.Id_info));
                    break;
                case ZAL.ACTUALITY_TYPE.ARTICLE:
                    item = new Article(Gateway.SelectClanek((int)Data.Id_clanek));
                    break;
                case ZAL.ACTUALITY_TYPE.RECORD:
                    item = new RecordAction(Gateway.SelectZapis((int)Data.Id_zapis));
                    break;
            }
            InitContent();//?
        }

        private void InitContent() {//?
            Data.Title = item.Title;
            Data.ShortText = item.GetShortText(0);
        }

        public void Synchronize() {
            ItemLazyLoad();
        }

        public static string CheckForChanges(User user, DateTime lastCheck) {
            return Gateway.CheckForChanges(user.Email, lastCheck);
        }

        public static Collection<Actuality> GetAllFor(User user) {
            Collection<AktualityTable> actualityValues = Gateway.SelectAllGeneralFor(user.Email);
            Collection<Actuality> actualities = new Collection<Actuality>();
            foreach (AktualityTable a in actualityValues) {
                actualities.Add(new Actuality(a));
            }
            return actualities;
        }

        public static List<int> GetChanged(User user, DateTime lastCheck) {
            return Gateway.GetChanged(user.Email, lastCheck);
        }

        public static Actuality Get(int id) {
            return new Actuality(Gateway.SelectGeneral(id));
        }

        public static bool Delete(Actuality actuality) {
            IntegrityCondition.UserIsLeader();
            return Gateway.Delete(actuality.Data.Id);
        }

        public XElement GetXml(string elementName) {
            XElement element = new XElement(elementName,
                new XElement("Id", Data.Id),
                new XElement("DateOfCreation", Data.DateOfCreation.Ticks),
                new XElement("Title", Data.Title),
                new XElement("ShortText", Data.ShortText),
                new XElement("Id_info", Data.Id_info),
                new XElement("Id_zapis", Data.Id_zapis),
                new XElement("Id_clanek", Data.Id_clanek));
            return element;
        }

        public static Actuality LoadFromXml(XElement element) {
            AktualityTable data = new AktualityTable {
                Id = Int32.Parse(element.Element("Id").Value),
                DateOfCreation = new DateTime(long.Parse(element.Element("DateOfCreation").Value)),
                Title = element.Element("Title").Value,
                ShortText = element.Element("ShortText").Value
            };
            if (!element.Element("Id_info").IsEmpty) {
                data.Id_info = Int32.Parse(element.Element("Id_info").Value);
            }
            else if (!element.Element("Id_zapis").IsEmpty) {
                data.Id_zapis = Int32.Parse(element.Element("Id_zapis").Value);
            }
            else if (!element.Element("Id_clanek").IsEmpty) {
                data.Id_clanek = Int32.Parse(element.Element("Id_clanek").Value);
            }
            return new Actuality(data);
        }
    }
}
