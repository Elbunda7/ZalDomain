using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class LogoutRequestModel
    {
        public int IdUser { get; set; }
        public string Token { get; set; }
    }
}
