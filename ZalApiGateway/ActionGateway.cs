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
        private JsonFormator jsonFormator;

        public ActionGateway() {
            jsonFormator = new JsonFormator(API.ENDPOINT.ACTIONS);
        }

        public async Task<ActionModel> GetAsync(int id) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET, id);
            tmp = await PclApiClient.Post(tmp);
            ActionModel model = JsonConvert.DeserializeObject<ActionModel>(tmp);
            return model;
        }

        public async Task<Collection<ActionModel>> GetAllAsync() {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET_ALL);
            tmp = await PclApiClient.Post(tmp);
            Collection<ActionModel> model = JsonConvert.DeserializeObject<Collection<ActionModel>>(tmp);
            return model;
        }

        public async Task<bool> AddAsync(ActionModel model) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.ADD, model);
            tmp = await PclApiClient.Post(tmp);
            model.Id = JsonConvert.DeserializeObject<int>(tmp);
            return true;
        }

        public async Task<bool> JoinAsync(int idAction, int idUser) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.JOIN, new Action_UserModel { Id_Action = idAction, Id_User = idUser });
            tmp = await PclApiClient.Post(tmp);
            bool result = JsonConvert.DeserializeObject<bool>(tmp);
            return result;
        }

        public async Task<bool> DeleteAsync(int idAction) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.DELETE, idAction);
            tmp = await PclApiClient.Post(tmp);
            bool result = JsonConvert.DeserializeObject<bool>(tmp);
            return result;
        }

        public List<string> GetParticipatingUsers(int id) {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ActionModel model) {
            throw new NotImplementedException();
        }

        public Task<ActionModel> GetChangedAsync(int id, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        public Task<Collection<ActionModel>> GetUpcomingAsync(int rankLevel, bool onlyOfficial) {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetAllChangedAsync(int rankLevel, DateTime lastCheck) {
            throw new NotImplementedException();
        }
    }
}
