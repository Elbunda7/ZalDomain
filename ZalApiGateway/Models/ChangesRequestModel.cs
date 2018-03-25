using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models
{
    public class ChangesRequestModel
    {
        public int Rank { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public DateTime LastCheck { get; set; }
    }
}
