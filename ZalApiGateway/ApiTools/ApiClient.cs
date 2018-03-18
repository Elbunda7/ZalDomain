using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ZalApiGateway.ApiTools
{
    internal class ApiClient
    {
        public static async Task<string> Post(string jsonContent) {
            var values = new Dictionary<string, string> {
                { "x", jsonContent.Encrypt() }
            };
            HttpContent content = new FormUrlEncodedContent(values);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync("http://zalesak.hlucin.com/api/endpoint.php", content);
            string str = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.InternalServerError) {
                throw new Exception($"Internal Server Error: \"{str}\"");
            }
            return str.Decrypt();
        }
    }
}
