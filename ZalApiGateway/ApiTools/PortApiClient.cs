using PortableRest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.ApiTools
{
    public class PortApiClient
    {
        /*public static async Task<string> Post(string jsonContent) {
            string str = jsonContent.Encrypt();
            RestClient client = new RestClient { BaseUrl = "http://zalesak.hlucin.com" };
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            RestRequest request = new RestRequest("http://zalesak.hlucin.com/api/endpoint.php", HttpMethod.Post);
            request.AddParameter("x", str);
            HttpResponseMessage response = await client.ExecuteAsync<HttpResponseMessage>(request);
            str = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.InternalServerError) {
                throw new Exception($"Internal Server Error: \"{str}\"");
            }
            return str.Decrypt();
        }*/
    }
}
