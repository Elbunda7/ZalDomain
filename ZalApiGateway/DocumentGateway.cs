using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ZalApiGateway.ApiTools;
using ZalApiGateway.Models;

namespace ZalApiGateway
{
    public class DocumentGateway
    {
        private JsonFormator jsonFormator;

        public DocumentGateway() {
            jsonFormator = new JsonFormator(API.ENDPOINT.ACTIONS);
        }

        public async Task<DocumentModel> GetAsync(int id) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET, id);
            tmp = await ApiClient.PostRequest(tmp);
            DocumentModel model = JsonConvert.DeserializeObject<DocumentModel>(tmp);
            return model;
        }

        public async Task<Collection<DocumentModel>> GetAllAsync() {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET_ALL);
            tmp = await ApiClient.PostRequest(tmp);
            Collection<DocumentModel> model = JsonConvert.DeserializeObject<Collection<DocumentModel>>(tmp);
            return model;
        }

    }
}
