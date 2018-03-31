using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public class LoginRespondModel
    {
        public bool IsExist { get; set; }
        public bool IsPasswordCorrect { get; set; }
        public bool HasAnyErrors => !(IsExist && IsPasswordCorrect);

        public UserModel UserModel { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
