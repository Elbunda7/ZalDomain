using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class ActionRequestModel
    {//spojit dědičností s ChangesRespondModel

        public int Rank { get; set; }
        public int Year { get; set; }
    }
}
