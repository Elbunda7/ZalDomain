using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models
{
    public class ChangesRespondModel<T> where T : IModel
    {
        public bool IsChanged { get; set; }
        public bool IsHardChanged { get; set; }
        public IEnumerable<int> Deleted { get; set; }
        public IEnumerable<T> Changed { get; set; }
        public IEnumerable<T> Added { get; set; }
    }
}
