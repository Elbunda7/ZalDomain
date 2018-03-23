using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System;
using ZalApiGateway.ApiTools;
using ZalApiGateway.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZalApiGateway
{
    public class BadgeGateway
    {
        private JsonFormator jsonFormator;

        public BadgeGateway() {
            jsonFormator = new JsonFormator(API.ENDPOINT.ACTIONS);
        }

        public async Task<BadgeModel> GetAsync(int id) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET, id);
            tmp = await ApiClient.PostRequest(tmp);
            BadgeModel model = JsonConvert.DeserializeObject<BadgeModel>(tmp);
            return model;
        }

        public async Task<Collection<BadgeModel>> GetAllAsync() {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET_ALL);
            tmp = await ApiClient.PostRequest(tmp);
            Collection<BadgeModel> model = JsonConvert.DeserializeObject<Collection<BadgeModel>>(tmp);
            return model;
        }

        public async Task<Collection<BadgeModel>> GetBadgesOwnedByUserAsync(string userEmail, bool haveThem) {
            throw new NotImplementedException();
        }

        public async Task<bool> GiveBadgeToUser(int idOdborka, string userEmail) {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveBadgeToUser(int idOdborka, string userEmail) {
            throw new NotImplementedException();
        }

    }
}
