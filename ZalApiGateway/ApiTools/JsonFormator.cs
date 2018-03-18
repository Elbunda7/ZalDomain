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
    public static class JsonFormator
    {

        public static string CreateApiRequestString(string endpoint, string operation, object content = null, string userToken = null) {
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
