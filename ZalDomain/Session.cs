using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;
using ZalDomain.consts;

namespace ZalDomain
{
    public class Session
    {
        public User CurrentUser { get; set; }
        public string Token { get; set; }
        public bool IsLogged => CurrentUser != null;
        public int UserRank => IsLogged ? CurrentUser.RankLevel : ZAL.RANK.CLEN;

        internal void Stop() {
            CurrentUser = null;
            Token = null;
        }
    }
}
