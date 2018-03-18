using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.ApiTools;
using ZalApiGateway.Models;

namespace ZalApiGateway
{
    public class ActionGateway
    {
        public async Task<ActionModel> GetAsync(int id) {
            string tmp = JsonFormator.CreateApiRequestString(API.ENDPOINT.ACTIONS, API.METHOD.GET, id);
            tmp = await ApiClient.Post(tmp);
            ActionModel model = JsonConvert.DeserializeObject<ActionModel>(tmp);
            return model;
        }

        public async Task<Collection<ActionModel>> GetAllAsync() {
            string tmp = JsonFormator.CreateApiRequestString(API.ENDPOINT.ACTIONS, API.METHOD.GET_ALL);
            tmp = await ApiClient.Post(tmp);
            Collection<ActionModel> model = JsonConvert.DeserializeObject<Collection<ActionModel>>(tmp);
            return model;
        }

        public async Task<bool> AddAsync(ActionModel model) {
            string tmp = JsonFormator.CreateApiRequestString(API.ENDPOINT.ACTIONS, API.METHOD.ADD, model);
            tmp = await ApiClient.Post(tmp);
            model.Id = JsonConvert.DeserializeObject<int>(tmp);
            return true;
        }

        public async Task<bool> JoinAsync(int idAction, int idUser) {
            string tmp = JsonFormator.CreateApiRequestString(API.ENDPOINT.ACTIONS, API.METHOD.JOIN, new Action_UserModel { Id_Action = idAction, Id_User = idUser });
            tmp = await ApiClient.Post(tmp);
            bool result = JsonConvert.DeserializeObject<bool>(tmp);
            return result;
        }

        public async Task<bool> DeleteAsync(int idAction) {
            string tmp = JsonFormator.CreateApiRequestString(API.ENDPOINT.ACTIONS, API.METHOD.DELETE, idAction);
            tmp = await ApiClient.Post(tmp);
            bool result = JsonConvert.DeserializeObject<bool>(tmp);
            return result;
        }
    }
}
