using ZalDomain.ActiveRecords;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ZalDomain.consts;

namespace ZalDomain.tools
{
    public class ActionObservableSortedSet : ObservableSortedSet<ActionEvent>
    {
        public ActionObservableSortedSet():base(new ActionComparer()) {}

        public ActionObservableSortedSet(IEnumerable<ActionEvent> enumerable) : base(enumerable, new ActionComparer()) { }        
    }
}
