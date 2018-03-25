using ZalDomain.consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;

namespace ZalDomain.tools
{
    internal static class UserPermision
    {
        internal static void HasRank(User user, int expectedRank) {
            if (user == null) {
                throw new UserPermisionException("null exception!");
            }
            if (user.RankLevel < expectedRank) {
                throw new UserPermisionException($"Low user Rank - expected: [{ZAL.RANK_NAME[expectedRank]}], current: [{ZAL.RANK_NAME[user.RankLevel]}]");
            }
        }
    }
}
