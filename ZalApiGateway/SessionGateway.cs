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
            model.Id = await SendRequestFor<int>(API.METHOD.REGISTER, model);
            return model.Id != -1;
        }

        public Task<LoginRespondModel> LoginAsync(LoginRequestModel model) {
            return SendRequestFor<LoginRespondModel>(API.METHOD.LOGIN, model);
        }

        public Task<TokenRespondModel> RefreshTokenAsync(TokenRequestModel model) {
            return SendRequestFor<TokenRespondModel>(API.METHOD.GET_TOKEN, model);
        }

        public Task LogoutAsync(LogoutRequestModel model) {
            return SendRequestFor<bool>(API.METHOD.LOGOUT, model);
        }
    }
}
