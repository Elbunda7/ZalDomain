using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class ChangesRespondModel<T> where T : IModel
    {
        public bool IsChanged { get; set; }
        public bool IsHardChanged { get; set; }
        public DateTime Timestamp { get; set; }
        public int[] Deleted { get; set; }
        public IEnumerable<T> Changed { get; set; }

        public ChangesRespondModel() {
            Changed = new List<T>();
        }

        public int[] GetDeleted() {
            return Deleted ?? new int[0];
        }

        public IEnumerable<T> GetChanged() {
            return Changed ?? new List<T>();
        }
    }
}
