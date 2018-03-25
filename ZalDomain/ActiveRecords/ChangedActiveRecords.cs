using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.ActiveRecords
{
    public class ChangedActiveRecords<T> where T : IActiveRecord
    {
        public IEnumerable<int> Deleted { get; set; }
        public IEnumerable<T> Changed { get; set; }
        public IEnumerable<T> Added { get; set; }
    }
}
