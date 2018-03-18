using DAL.Gateway;
using DAL.tableStructures;
using ZalDomain.consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.ActiveRecords
{
    public class GuestUser:IActiveRecord
    {
        private UzivatelMainTable MainData;
        private UzivatelDetailTable otherData;
        private UzivatelDetailTable OtherData { get { return OtherDataLazyLoad(); } set { otherData = value; } }

        public string Email { get { return ""; } }
        public string ShortName { get { return MainData.ShortName; } }
        public int RankLevel { get { return ZAL.RANK.NOVACEK; } }
        public string Rank { get { return ZAL.RANK_NAME[RankLevel]; } }
        public string Name { get { return OtherData.Jmeno; } }
        public string Surname { get { return OtherData.Prijmeni; } }
        public string Nick { get { return OtherData.Prezdivka; } } //nick == shortName ?
        public string Phone { get { return ""; } }

        private static UzivatelGateway gateway;
        private static UzivatelGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = UzivatelGateway.GetInstance();
                }
                return gateway;
            }
        }

        private UzivatelDetailTable OtherDataLazyLoad() {
            if (otherData == null) {
                otherData = Gateway.SelectDetail(MainData.Email);
            }
            return otherData;
        }

        internal GuestUser(UzivatelTable item) {
            MainData = item.Main;
            OtherData = item.Minor;
        }
    }
}
