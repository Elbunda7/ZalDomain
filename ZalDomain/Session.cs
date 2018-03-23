using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;

namespace ZalDomain
{
    public class Session
    {
        public User CurrentUser { get; set; }
        public string Token { get; set; }
        public bool IsLogged { get; set; }

        internal void Stop() {
            CurrentUser = null;
            Token = null;
            IsLogged = false;
        }
    }
}
