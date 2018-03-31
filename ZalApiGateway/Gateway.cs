using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.ApiTools;

namespace ZalApiGateway
{
    public class Gateway
    {
        protected JsonFormator JsonFormator { get; set; }

        protected Gateway(string ApiEndpoint) {
            JsonFormator = new JsonFormator(ApiEndpoint);
        }

        protected async Task<T> SendRequestFor<T>(string apiMethod, object content = null, string userToken = null) {
            string tmp = JsonFormator.CreateApiRequestString(apiMethod, content, userToken);
            tmp = await ApiClient.PostRequest(tmp);
            return JsonConvert.DeserializeObject<T>(tmp);
        }
    }
}
