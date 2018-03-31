using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.ApiTools;
using ZalApiGateway.Models.ApiCommunicationModels;

namespace ZalApiGateway
{
    public class SessionGateway : Gateway
    {
        public SessionGateway() : base(API.ENDPOINT.SESSION) { }

        public async Task<bool> RegisterAsync(RegistrationRequestModel model) {
            int respond = await SendRequestFor<int>(API.METHOD.REGISTER, model);
            model.Id = respond;
            return respond != -1;
        }

        public Task<LoginRespondModel> LoginAsync(LoginRequestModel model) {
            return SendRequestFor<LoginRespondModel>(API.METHOD.LOGIN, model);
        }
    }
}
