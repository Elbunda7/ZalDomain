using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class SessionRequestModel
    {
        public int IdUser { get; set; }
        public string RefreshToken { get; set; }
    }
}
