using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.ApiTools
{
    public class RestApiClient
    {
        public static async Task<string> Post(string jsonContent) {
            string str = jsonContent.Encrypt();
            RestClient client = new RestClient("http://zalesak.hlucin.com");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            RestRequest request = new RestRequest("http://zalesak.hlucin.com/api/endpoint.php", Method.POST);
            request.AddParameter("x", str);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            str = response.Content.Decrypt();
            if (response.StatusCode == HttpStatusCode.InternalServerError) {
                throw new Exception($"Internal Server Error: \"{str}\"");
            }
            return str;
        }
    }   
}
