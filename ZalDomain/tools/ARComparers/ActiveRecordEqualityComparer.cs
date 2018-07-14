using System.Collections.Generic;
using ZalDomain.ActiveRecords;

namespace ZalDomain.tools.ARComparers
{
    public class ActiveRecordEqualityComparer : IEqualityComparer<IActiveRecord>
    {
        private static ActiveRecordEqualityComparer instance;
        public static ActiveRecordEqualityComparer Instance => instance ?? (instance = new ActiveRecordEqualityComparer());

        public bool Equals(IActiveRecord x, IActiveRecord y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(IActiveRecord obj) {
            return obj.GetHashCode();
        }
    }
}
