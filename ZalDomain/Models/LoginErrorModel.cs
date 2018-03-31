using ZalApiGateway.Models.ApiCommunicationModels;

namespace ZalDomain.Models
{
    public class LoginErrorModel
    {
        public bool IsExist { get; set; }
        public bool IsPasswordCorrect { get; set; }
        public bool HasAnyErrors => !(IsExist && IsPasswordCorrect);

        public LoginErrorModel(LoginRespondModel model) {
            IsExist = model.IsExist;
            IsPasswordCorrect = model.IsPasswordCorrect;
        }
    }
}
