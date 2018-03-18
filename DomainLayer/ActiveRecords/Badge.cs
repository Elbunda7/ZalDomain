using DAL.Gateway;
using DAL.tableStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.ActiveRecords
{
    public class Badge : IActiveRecord, ISimpleItem
    {
        private OdborkyTable Data;

        public int Id { get { return Data.Id; } }
        public string Title { get { return Data.Jmeno; } }
        public string Text { get { return Data.Popis; } }


        private static OdborkyGateway gateway;
        private static OdborkyGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = OdborkyGateway.GetInstance();
                }
                return gateway;
            }
        }

        public static Collection<Badge> GetAll() {
            Collection<OdborkyTable> rawData = Gateway.GetAll();
            return InitializeForAll(rawData);
        }

        private static Collection<Badge> InitializeForAll(Collection<OdborkyTable> items) {
            Collection<Badge> odborky = new Collection<Badge>();
            foreach(OdborkyTable o in items) {
                odborky.Add(new Badge(o));
            }
            return odborky;
        }

        private Badge(OdborkyTable data) {
            Data = data;
        }

        public static Collection<Badge> GetAcquired(User uzivatel) {
            Collection<OdborkyTable> rawData = Gateway.UserOwns(uzivatel.Email, true);
            return InitializeForAll(rawData);
        }

        public static Collection<Badge> GetNotAcquired(User uzivatel) {
            Collection<OdborkyTable> rawData = Gateway.UserOwns(uzivatel.Email, false);
            return InitializeForAll(rawData);
        }

        public override string ToString() {
            return Data.Jmeno;
        }
    }
}
