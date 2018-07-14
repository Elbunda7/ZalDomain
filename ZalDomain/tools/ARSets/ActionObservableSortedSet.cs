using System.Collections.Generic;
using ZalDomain.ActiveRecords;
using ZalDomain.tools.ARComparers;

namespace ZalDomain.tools.ARSets
{
    public class ActionObservableSortedSet : ObservableSortedSet<ActionEvent>
    {
        public ActionObservableSortedSet():base(new ActionComparer()) {}

        public ActionObservableSortedSet(IEnumerable<ActionEvent> enumerable) : base(enumerable, new ActionComparer()) { }        
    }
}
