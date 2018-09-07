using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ZalDomain.tools;
using ZalDomain.consts;
using ZalApiGateway.Models;
using ZalApiGateway;
using System.Threading.Tasks;
using ZalDomain.Models;
using ZalApiGateway.Models.ApiCommunicationModels;

namespace ZalDomain.ActiveRecords
{
    public class Article : IActiveRecord, ISimpleItem {

        private ArticleModel model;
        private User author;

        public int Id => model.Id;
        public string Title => model.Title;
        public string Text => model.Text;
        public int Id_Author => model.Id_Author;
        public DateTime Date_Creation => model.Date;
        public int Id_Gallery => model.Id_Gallery;
        //public string ShortText { get { return model.ShortText; } }
        //public string Type { get { return GetItemType(); } }        //zrušit 3x null-ids, zařídit typ+id
        /*public int RankLeast { get { return Data.Od_hodnosti; } }
        public int? ForGroup { get { return Data.Pro_druzinu; } }*/

        private static ArticleGateway gateway;
        private static ArticleGateway Gateway => gateway ?? (gateway = new ArticleGateway());

        private UnitOfWork<ArticleUpdateModel> unitOfWork;
        public UnitOfWork<ArticleUpdateModel> UnitOfWork => unitOfWork ?? (unitOfWork = new UnitOfWork<ArticleUpdateModel>(model, OnUpdateCommited));

        private Task<bool> OnUpdateCommited() {
            return Gateway.UpdateAsync(model, Zal.Session.Token);
        }

        private async Task<User> AuthorLazyLoad() {
            if (author == null) {
                author = await Zal.Users.Get(Id_Author);
            }
            return author;
        }

        public static async Task<Article> AddAsync(User author, string title, string text) {
            ArticleModel model = new ArticleModel {
                Id_Author = author.Id,
                Title = title,
                Text = text
            };
            if (await Gateway.AddAsync(model)) {
                return new Article(model);
            }
            return null;
        }

        public Article(ArticleModel model) {
            this.model = model;
        }

        public static async Task<string> CheckForChanges(User user, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        public static async Task<bool> LoadTopTen(int[] ids, DateTime timestamp) {
            var requestModel = new ArticleTopTenRequestModel {
                Ids = new int[] { 6, 24, 22, 20, 18, 16, 14, 1, 10, 3 },
                Timestamp = new DateTime(2018, 10, 8),
            };
            var a = await Gateway.LoadIfChangedTopTenAsync(requestModel, "str");
            return true;
        }

        public static async Task<Collection<Article>> GetAllFor(int userRank) {
            /* Collection<AktualityTable> actualityValues = Gateway.SelectAllGeneralFor(user.Email);
             Collection<Actuality> actualities = new Collection<Actuality>();
             foreach (AktualityTable a in actualityValues) {
                 actualities.Add(new Actuality(a));
             }
             return actualities;*/
            throw new NotImplementedException();
        }

        public static async Task<List<int>> GetChanged(User user, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        public static async Task<Article> GetAsync(int id) {
            return new Article(await Gateway.GetAsync(id));
        }

        public static async Task<bool> Delete(Article actuality) {
            UserPermision.HasRank(Zal.Session.CurrentUser, ZAL.Rank.Vedouci);
            return await Gateway.DeleteAsync(actuality.model.Id);
        }

        public XElement GetXml(string elementName) {
            /*XElement element = new XElement(elementName,
                new XElement("Id", model.Id),
                new XElement("DateOfCreation", model.DateOfCreation.Ticks),
                new XElement("Title", model.Title),
                new XElement("ShortText", model.ShortText),
                new XElement("Id_info", model.Id_info),
                new XElement("Id_zapis", model.Id_zapis),
                new XElement("Id_clanek", model.Id_clanek));
            return element;*/

            throw new NotImplementedException();
        }

        public static Article LoadFromXml(XElement element) {
            /*AktualityTable data = new AktualityTable {
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
            return new Actuality(data);*/

            throw new NotImplementedException();
        }
    }
}
