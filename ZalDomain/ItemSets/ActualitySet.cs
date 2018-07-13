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
    public class ActualitySet {
        private ActualityObservableSortedSet Data { get; set; }
        private DateTime LastCheck;

        public ActualitySet() {
            Data = new ActualityObservableSortedSet();
            LastCheck = ZAL.DATE_OF_ORIGIN;
        }

        internal async Task<Article> CreateNewArticle(string title, string text, int fromRank, int? forGroup = null) {
            //token uživatele
            return await CreateNewArticle(Zal.Session.CurrentUser, title, text, fromRank, forGroup);
        }

        internal async Task<Article> CreateNewArticle(User author, string title, string text, int fromRank, int? forGroup = null) {
            Article article = await Article.AddAsync(author, title, text);//, fromRank, forGroup));
            if (article != null) {
                Data.Add(article);
                return article;
            }
            return null;
        }

        public async Task<bool> AddNewArticle(string title, string text, int fromRank, int? forGroup = null) {
            Article article = await CreateNewArticle(Zal.Session.CurrentUser, title, text, fromRank, forGroup);
            return article != null;
        }

        public async void Remove(Article item) {
            //if (Zal.Me.IsLeader()) { }
            if (await Article.Delete(item)) {
                Data.Remove(item);
            }
            else {
                throw new Exception("Nejde odstranit");
            }
        }

        public async Task SynchronizeAsync() {
            DateTime tmp = DateTime.Now;
            CheckForChanges();
            LastCheck = tmp;
        }

        internal void ReSynchronize() {
            LastCheck = ZAL.DATE_OF_ORIGIN;
            SynchronizeAsync();
        }

        private async void CheckForChanges() {
            Data.Clear();
            Data.AddAll(await Article.GetAllFor((int)Zal.Session.UserRank));
            /*string changes = Article.CheckForChanges(Zal.Me, LastCheck);
            if (changes.Equals(CONST.CHANGES.MAJOR)) {
                Data.Clear();
                Data.AddAll(Article.GetAllFor(Zal.Me));
            }
            else if (changes.Equals(CONST.CHANGES.MINOR)) {
                List<int> changedItems = Article.GetChanged(Zal.Me, LastCheck);
                foreach (int id in changedItems) {
                    if (id < 0) {
                        Data.RemoveById(-id);
                    }
                    else {
                        Data.RemoveById(id);
                        Data.Add(Article.Get(id));
                    }
                }
            }*/
        }

        public async Task<Article> GetArticleAsync(int id) {
            Article a;
            if (Data.Any(article => article.Id == id)) {
                a = Data.Single(article => article.Id == id);
            }
            else {
                a = await Article.GetAsync(id);
                Data.Add(a);
            }
            return a;
        }    

        //public IActualityItem Get(Article a) {
        //    return a.ItemLazyLoad();
        //}

        public IEnumerable<Article> GetAll() {
            SynchronizeAsync();
            return Data;
        }

        internal XElement GetXml(string elementName) {
            XElement element = new XElement(elementName);
            foreach (Article a in Data) {
                element.Add(a.GetXml("Actuality"));
            }
            return element;
        }

        internal void LoadFromXml(XElement element) {
            IEnumerable<XElement> data = element.Elements("Actuality");
            foreach (XElement el in data) {
                Article actuality = Article.LoadFromXml(el);
                if (!Data.Contains(actuality)){ //contains vs containsById
                    Data.Add(Article.LoadFromXml(el));
                }
            }
        }
    }
}