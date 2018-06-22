﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ZalApiGateway.ApiTools;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;

namespace ZalApiGateway
{
    public class ActionGateway:Gateway
    {
        public ActionGateway() : base(API.ENDPOINT.ACTIONS) { }

        public Task<ActionModel> GetAsync(int id) {
            return SendRequestFor<ActionModel>(API.METHOD.GET, id);
        }

        public Task<IEnumerable<ActionModel>> GetAllAsync() {
            return SendRequestFor<IEnumerable<ActionModel>>(API.METHOD.GET_ALL);
        }

        public Task<AllRespondModel<ActionModel>> GetPastByYearAsync(ActionRequestModel model, string token) {
            return SendRequestFor<AllRespondModel<ActionModel>>(API.METHOD.GET_PAST_BY_YEAR, model, token);
        }

        public async Task<bool> AddAsync(ActionModel model, string token) {
            int respond = await SendRequestFor<int>(API.METHOD.ADD, model, token);
            model.Id = respond;
            return respond != -1;
        }

        public Task<bool> Join(ActionUserJoinModel model) {
            return SendRequestFor<bool>(API.METHOD.JOIN, model);
        }

        public Task<bool> UnJoin(ActionUserModel model) {
            return SendRequestFor<bool>(API.METHOD.UNJOIN, model);
        }

        public Task<bool> DeleteAsync(int idAction, string token) {
            return SendRequestFor<bool>(API.METHOD.DELETE, idAction, token);
        }

        public Task<IEnumerable<MembersOnActionModel>> GetUsersOnAction(int id) {
            return SendRequestFor<IEnumerable<MembersOnActionModel>>(API.METHOD.GET_USERS_ON_ACTION, id);
        }

        public Task<bool> UpdateAsync(ActionModel model, string token) {
            return SendRequestFor<bool>(API.METHOD.UPDATE, model, token);
        }

        public async Task<ActionModel> GetChangedAsync(int id, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        public Task<ChangesRespondModel<ActionModel>> GetAllChangedAsync(ChangesRequestModel model, string token) {
            return SendRequestFor<ChangesRespondModel<ActionModel>>(API.METHOD.GET_CHANGED, model, token);
        }
    }
}
