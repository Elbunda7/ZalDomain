﻿using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZalApiGateway.ApiTools
{
    internal class ApiClient
    {
        private const string TokenExpired = "421";

        static Uri BaseUri = new Uri("http://zalesak.hlucin.com");
        static Uri ResourceUri = new Uri("http://zalesak.hlucin.com/api/index.php");

        public static async Task<string> PostRequest(string jsonContent) {
            string str = jsonContent.Encrypt();
            using (RestClient client = new RestClient(BaseUri)) {
                RestRequest request = new RestRequest(ResourceUri, Method.POST);
                request.AddParameter("x", str);
                str = await SendAsync(client, request);
            }
            return str.Decrypt();
        }

        private static async Task<string> SendAsync(RestClient client, RestRequest request) {
            IRestResponse<string> response;
            try {
                response = await client.Execute<string>(request);
            }
            catch (Exception e) {
                if (e.Message.Contains(TokenExpired)) {
                    throw new HttpRequestException("Token expired (response code: 421)");
                }
                throw e;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) {
                throw new NoChangesException();
            }
            return response.Content;
        }
    }
}
