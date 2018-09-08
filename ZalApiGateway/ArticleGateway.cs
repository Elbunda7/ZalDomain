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
using ZalApiGateway.Models.ApiCommunicationModels;

namespace ZalApiGateway
{
    public class ArticleGateway:Gateway
    {
        public ArticleGateway() : base(API.ENDPOINT.ARTICLES){ }


        public async Task<bool> AddAsync(ArticleModel model) {
            model.Id = await SendRequestFor<int>(API.METHOD.ADD, model);
            return model.Id != -1;
        }

        public async Task<bool> AddAsync(ExtendedArticleModel model) {
            model.Id = await SendRequestFor<int>(API.METHOD.ADD, model);
            return model.Id != -1;
        }

        public Task<bool> DeleteAsync(int id) {
            return SendRequestFor<bool>(API.METHOD.DELETE, id);
        }

        public Task<IEnumerable<ArticleModel>> LoadNextAsync(int model) {
            return SendRequestFor<IEnumerable<ArticleModel>>(API.METHOD.LOAD_NEXT, model);
        }

        public async Task<ArticlesChangesRespondModel> LoadIfChangedTopTenAsync(ArticleTopTenRequestModel model, string token) {
            var respond = await SendRequestForNullable<ArticlesChangesRespondModel>(API.METHOD.LOAD_TOP_TEN, model, token);
            return respond ?? new ArticlesChangesRespondModel();
        }

        public ArticleModel SelectGeneral(int id) {
            throw new NotImplementedException();
        }

        public async Task<Collection<ArticleModel>> SelectAllGeneralFor(string uzivatelEmail) {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ArticleModel model, string token) {
            return SendRequestFor<bool>(API.METHOD.UPDATE, model, token);
        }

        public async Task<ArticleModel> GetAsync(int id) {
            throw new NotImplementedException();
        }
    }
}
