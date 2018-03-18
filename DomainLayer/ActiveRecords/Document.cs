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
    public class Document : IActiveRecord, ISimpleItem
    {
        private DokumentyTable Data;

        public int Id { get { return Data.Id; } }
        public string Title { get { return Data.Jmeno; } }
        public string Text { get { return Data.Text; } }

        private static DokumentyGateway gateway;
        private static DokumentyGateway Gateway {
            get {
                if (gateway == null) {
                    gateway = DokumentyGateway.GetInstance();
                }
                return gateway;
            }
        }

        public Document(DokumentyTable doc) {
            Data = doc;
        }

        public static Collection<Document> GetAll() {
            Collection<DokumentyTable> rawData = Gateway.GetAll();
            return InitializeForAll(rawData);
        }

        private static Collection<Document> InitializeForAll(Collection<DokumentyTable> items) {
            Collection<Document> document = new Collection<Document>();
            foreach (DokumentyTable doc in items) {
                document.Add(new Document(doc));
            }
            return document;
        }

        public override string ToString() {
            return Data.Jmeno;
        }
    }
}
