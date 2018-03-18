using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.tableStructures;
using DAL.Gateway;
using ZalDomain.consts;

namespace ZalDomain.ActiveRecords
{
    public class Article:IActualityItem
    {
        internal ClanekTable Data;
        private User author;

        public int Id { get { return Data.Id; } }
        public User Author { get { return AuthorLazyLoad(); } private set { author = value; } }
        public string Title {get {return Data.Nazev;}}
        public string Text { get { return Data.Text; } }
        public string Type {get {return ZAL.ACTUALITY_TYPE.ARTICLE;}}
        public int RankLeast { get { return Data.Od_hodnosti; } }
        public int? ForGroup { get { return Data.Pro_druzinu; } }


        private static AktualityGateway gateway;
        private static AktualityGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = AktualityGateway.GetInstance();
                }
                return gateway;
            }
        }



        private User AuthorLazyLoad() {
            if (author == null) {
                author = Zal.Users.GetByEmail(Data.AuthorEmail);
            }
            return author;
        }


        internal Article(ClanekTable item) {
            Data = item;
        }

        internal Article(User author, string title, string text, int fromRank, int? forGroup = null) {
            Author = author;
            Data = new ClanekTable {
                Id = -1,
                Id_aktuality = -1,
                AuthorEmail = author.Email,
                Nazev = title,
                Text = text,
                Od_hodnosti = fromRank,
                Pro_druzinu = forGroup
            };
        }

        public void Aktualize(String title, String text, int? odHodnosti) {
            Aktualize(title, text, odHodnosti, Data.Pro_druzinu);
        }

        public void Aktualize(String title, String text, int? odHodnosti, int? proDruzinu) {
            if (title != null) {
                Data.Nazev = title;
            }
            if (text != null) {
                Data.Text = text;
            }
            if (odHodnosti != null) {
                Data.Od_hodnosti = (int)odHodnosti;
            }
            Data.Pro_druzinu = proDruzinu;
            Gateway.Update(Data);
        }

        public string GetShortText(int length) {
            return Data.Text;
        }
    }
}
