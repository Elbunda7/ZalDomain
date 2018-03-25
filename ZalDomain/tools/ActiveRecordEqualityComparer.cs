using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;

namespace ZalDomain.tools
{
    public class ActiveRecordEqualityComparer : IEqualityComparer<IActiveRecord>
    {
        public bool Equals(IActiveRecord x, IActiveRecord y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(IActiveRecord obj) {
            return obj.GetHashCode();
        }
    }
}
