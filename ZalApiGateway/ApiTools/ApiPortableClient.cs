using PortableRest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.ApiTools
{
    public class ApiPortableClient
    {
        public static async Task<string> Post(string jsonContent) {

            string str = jsonContent.Encrypt();

            var client = new RestClient { BaseUrl = "http://zalesak.hlucin.com"};
            var request = new RestRequest("http://zalesak.hlucin.com/api/endpoint.php", HttpMethod.Post);
            request.AddParameter("x", str);

            var result = await client.SendAsync<string>(request);
            str  = result.Content.Decrypt();

            return str;
        }
    }
}
