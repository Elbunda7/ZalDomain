using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class ActionRequestModel
    {
        public int Rank { get; set; }
        public int Year { get; set; }
    }

    public class ActionChangesRequestModel : ActionRequestModel, IChangesRequestModel
    {
        public int Count { get; set; }
        public DateTime LastCheck { get; set; }
    }
}
