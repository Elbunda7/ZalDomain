using ZalDomain.ActiveRecords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZalDomain.tools
{
    class ActualityObservableSortedSet:ObservableSortedSet<Article>
    {
        public ActualityObservableSortedSet():base(new ActualityComparer()) {

        }

        public bool RemoveById(int id) {
            foreach(Article a in this) {
                if (a.Id == id) {
                    Remove(a);
                    return true;
                }
            }
            return false;
        }

        [Obsolete]
        public bool ContainsById(Article item) {
            foreach (Article a in this) {
                if (a.Id == item.Id) {
                    return true;
                }
            }
            return false;
        }
       
    }
}