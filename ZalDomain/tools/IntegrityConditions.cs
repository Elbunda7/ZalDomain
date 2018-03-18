using ZalDomain.consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.tools
{
    internal static class IntegrityCondition
    {
        internal static void UserIsLeader() {
            if (Zal.Me.RankLevel < ZAL.RANK.VEDOUCI) {
                throw new Exception("uživatel není vedoucí");
            }
        }

        internal static void UserHasSecondRank() {
            if (Zal.Me.RankLevel < ZAL.RANK.PODRADCE) {
                throw new Exception("uživatel není podrádce");
            }
        }
    }
}
