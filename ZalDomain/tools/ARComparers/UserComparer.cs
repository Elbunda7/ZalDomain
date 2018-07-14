using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;

namespace ZalDomain.tools.ARComparers
{
    public class UserComparer:Comparer<User>
    {
        public override int Compare(User x, User y) {
            int comparison = Comparer<int>.Default.Compare((int)x.Group, (int)y.Group);
            if (comparison == 0) {
                comparison = Comparer<int>.Default.Compare((int)x.Rank, (int)y.Rank);
            }
            if (comparison == 0) {
                comparison = Comparer<string>.Default.Compare(x.Nick, y.Nick);
            }
            if (comparison == 0) {
                comparison = Comparer<int>.Default.Compare(x.Id, y.Id);
            }
            return comparison;
        }
    }
}
