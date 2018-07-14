using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class UserRequestModel
    {
        public int Groups { get; set; }
        public int Ranks { get; set; }
        public int Roles { get; set; }
    }

    public class UserChangesRequesModel : UserRequestModel, IChangesRequestModel
    {
        public int Count { get; set; }
        public DateTime LastCheck { get; set; }
    }
}
