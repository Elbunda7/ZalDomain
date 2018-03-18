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
    public class InfoAction:IActualityItem
    {
        internal Informace_o_akciTable Data;

        public int Id { get { return Data.Id_akce; } }
        public string Title { get { return "informace"; } }
        public string Type {get {return ZAL.ACTUALITY_TYPE.INFO;}}
        public string Text { get { return Data.Text; } }
        public int Price { get { return Data.Price; } }
        public decimal GPS_lon { get { return Data.GPS_lon; } }
        public decimal GPS_lat { get { return Data.GPS_lat; } }


        private static AktualityGateway gateway;
        private static AktualityGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = AktualityGateway.GetInstance();
                }
                return gateway;
            }
        }        


        internal InfoAction(Informace_o_akciTable item) {
            Data = item;
        }

        internal InfoAction(string text, decimal gpsLon, decimal gpsLat, int idAkce, int price) {//idAkce nesmí být -1
            Data = new Informace_o_akciTable {
                Id_aktuality = -1,
                Text = text,
                GPS_lon = gpsLon,
                GPS_lat = gpsLat,
                Id_akce = idAkce,
                Price = price
            };
        }

        public void Aktualize(String text, int? price, decimal? gpsLon, decimal? gpsLat) {
            if (text != null) {
                Data.Text = text;
            }
            if (price != null) {
                Data.Price = (int)price;
            }
            if (gpsLon != null) {
                Data.GPS_lon = (decimal)gpsLon;
            }
            if (gpsLat != null) {
                Data.GPS_lat = (decimal)gpsLat;
            }
            Gateway.Update(Data);
        }

        public string GetShortText(int length) {
            return Data.Text;
        }
    }
}
