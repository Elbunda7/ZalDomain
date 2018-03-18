using ZalDomain.ActiveRecords;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ZalDomain.tools
{
    class ActionObservableSortedSet : ObservableSortedSet<ActionEvent>
    {

        public ActionObservableSortedSet():base(new ActionComparer()) {}
        
        public bool RemoveById(int id) {
            foreach (ActionEvent a in this) {
                if (a.Id == id) {
                    Remove(a);
                    return true;
                }
            }
            return false;
        }
    }
}
