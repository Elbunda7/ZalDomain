using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.NonSqlModels
{
    public class ChangesRespondModel<T> where T : IModel
    {
        public bool IsChanged { get; set; }
        public bool IsHardChanged { get; set; }

        private IEnumerable<int> deleted;
        public IEnumerable<int> Deleted {
            get { return deleted ?? new List<int>(); }
            set { deleted = value; }
        }

        public IEnumerable<T> changed;
        public IEnumerable<T> Changed {
            get { return changed ?? new List<T>(); }
            set { changed = value; }
        }
    }
}
