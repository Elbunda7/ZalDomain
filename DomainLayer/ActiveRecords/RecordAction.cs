using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DAL.tableStructures;
using DAL.Gateway;
using ZalDomain.consts;

namespace ZalDomain.ActiveRecords
{
    public class RecordAction : IActualityItem
    {
        internal Zapis_z_akceTable Data;

        public int Id { get { return Data.Id_akce; } }
        public string Title { get { return "zapis"; } }//získat z databáze
        public string Text { get { return Data.Text; } }
        public string Type { get { return ZAL.ACTUALITY_TYPE.RECORD; } }

        private static AktualityGateway gateway;
        private static AktualityGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = AktualityGateway.GetInstance();
                }
                return gateway;
            }
        }

        internal RecordAction(Zapis_z_akceTable item) {
            Data = item;
        }

        internal RecordAction(string text, int idAkce) {
            Data = new Zapis_z_akceTable {
                Id_akce = idAkce,
                Id_aktuality = -1,
                Text = text
            };
        }

        public void Aktualize(String text) {
            Data.Text = text;
            Gateway.Update(Data);
        }

        public string GetShortText(int length) {
            return Data.Text;
        }
    }
}
