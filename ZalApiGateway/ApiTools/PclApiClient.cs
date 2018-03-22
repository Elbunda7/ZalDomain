using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.ApiTools
{
    internal class PclApiClient
    {
        static Uri BaseUri = new Uri("http://zalesak.hlucin.com");
        static Uri ResourceUri = new Uri("http://zalesak.hlucin.com/api/endpoint.php");

        public static async Task<string> Post(string jsonContent) {
            string str = jsonContent.Encrypt();
            using (RestClient client = new RestClient(BaseUri)) {
                RestRequest request = new RestRequest(ResourceUri, Method.POST);
                request.AddParameter("x", str);
                var result = await client.Execute<string>(request);
                str = result.Content;
                if (result.StatusCode == HttpStatusCode.InternalServerError) {
                    throw new Exception($"Internal Server Error: \"{str}\"");
                }
            }
            return str.Decrypt();
        }
    }
}
