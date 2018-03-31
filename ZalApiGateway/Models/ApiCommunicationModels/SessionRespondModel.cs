using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class SessionRespondModel
    {
        public bool IsExpired { get; set; }
        public string Token { get; set; }
    }
}
