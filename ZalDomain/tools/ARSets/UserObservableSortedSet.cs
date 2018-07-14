using System.Collections.Generic;
using ZalDomain.ActiveRecords;
using ZalDomain.tools.ARComparers;

namespace ZalDomain.tools.ARSets
{
    public class UserObservableSortedSet:ObservableSortedSet<User>
    {
        public UserObservableSortedSet() : base(new UserComparer()) { }

        public UserObservableSortedSet(IEnumerable<User> enumerable) : base(enumerable, new UserComparer()) { }
    }
}
