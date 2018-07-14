using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using ZalApiGateway.ApiTools;
using System.Threading.Tasks;
using ZalApiGateway.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using ZalApiGateway.Models.ApiCommunicationModels;

namespace ZalApiGateway
{
    public class UserGateway : Gateway {

        public UserGateway() : base(API.ENDPOINT.USERS) { }

        public Task<UserModel> GetAsync(int id) {
            return SendRequestFor<UserModel>(API.METHOD.GET, id);
        }

        public Task<AllRespondModel<UserModel>> GetAllAsync(UserRequestModel model) {
            return SendRequestFor<AllRespondModel<UserModel>>(API.METHOD.GET_ALL, model);
        }

        public async Task<bool> AddAsync(UserModel model, string token) {//nastaví správně id u modelu?
            int respond = await SendRequestFor<int>(API.METHOD.ADD, model, token);
            model.Id = respond;
            return respond != -1;
        }

        public Task<bool> DeleteAsync(int id, string token) {
            return SendRequestFor<bool>(API.METHOD.DELETE, id, token);
        }

        public bool CheckForChanges(int numOfUsers, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        [Obsolete]
        public async Task<bool> InsertBadget(string userEmail, int idOdborka) {
            throw new NotImplementedException();
        }

        //todo bodíky

        public bool BecomeMember(UserModel user) {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UserModel user, string token) {
            return SendRequestFor<bool>(API.METHOD.UPDATE, user, token);
        }

        public bool Update(string email, bool isPaid) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserModel>> GetAsync(IEnumerable<int> ids) {
            throw new NotImplementedException();
        }

        /*public Collection<Uzivatel> getAllFrom(int id_druziny) {
            SqlCommand command = db.CreateCommand(SQL_SELECT + " WHERE Druzina_id_druzina = " + id_druziny);
            SqlDataReader reader = db.Select(command);
            return ReadOneFrom(reader);
        }*/

        /*public int Delete(int id) {
            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
            command.Parameters.Add(SetParam("@id_uzivatel", id);
            return db.ExecuteNonQuery(command);
        }*/

        /*public Collection<Uzivatel> getParticipatingAt(Akce akce) {
            SqlCommand command = db.CreateCommand(SQL_SELECT_USERS_AT_ACTION);
            command.Parameters.Add(SetParam("@id_akce", akce.Id);
            SqlDataReader reader = db.Select(command);
            return ReadAllFrom(reader);
        }*/

        //public  Collection<Uzivatel> getParticipatingAt(Akce akce, Druzina druzina)
        //{
        //    SqlCommand command = db.CreateCommand(SQL_SELECT_USERS_AT_ACTION + WHERE_DRUZINA);
        //    command.Parameters.Add(SetParam("@id_akce", akce.Id);
        //    command.Parameters.Add(SetParam("@id_druzina", druzina.Id);
        //    SqlDataReader reader = db.Select(command);
        //    return ReadAllFrom(reader);
        //}
    }
}
