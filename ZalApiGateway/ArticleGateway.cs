using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway.ApiTools;
using ZalApiGateway.Models;

namespace ZalApiGateway
{
    public class ArticleGateway:Gateway
    {
        public ArticleGateway() : base(API.ENDPOINT.ARTICLES){ }


        public async Task<bool> AddAsync(ArticleModel model) {
            int respond = await SendRequestFor<int>(API.METHOD.REGISTER, model);
            model.Id = respond;
            return respond != -1;
        }

        public Task<bool> DeleteAsync(int id) {
            return SendRequestFor<bool>(API.METHOD.DELETE, id);
        }

        public async Task<string> CheckForChanges(string userEmail, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        public async Task<List<int>> GetChanged(string userEmail, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        public ArticleModel SelectGeneral(int id) {
            throw new NotImplementedException();
        }

        public async Task<Collection<ArticleModel>> SelectAllGeneralFor(string uzivatelEmail) {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ArticleModel model) {
            throw new NotImplementedException();
        }

        public async Task<ArticleModel> GetAsync(int id) {
            throw new NotImplementedException();
        }

    }
}
