using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.Models;

namespace ZalApiGateway.ApiTools
{
    public class JsonFormator {

        private string endpoint;

        internal JsonFormator(string endpoint) {
            this.endpoint = endpoint;
        }

        internal string CreateApiRequestString(string operation, object content = null, string userToken = null) {
            JObject requestObject = new JObject {
                { "endpoint", endpoint },
                { "operation", operation }
            };
            if (userToken != null) requestObject.Add("token", userToken);
            if (content != null) requestObject.Add("body", JToken.FromObject(content));
            return requestObject.ToString();
        }
    }
}
