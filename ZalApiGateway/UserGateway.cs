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
    public class UserGateway {

        private JsonFormator jsonFormator;

        public UserGateway() {
            jsonFormator = new JsonFormator(API.ENDPOINT.USERS);
        }

        public async Task<UserModel> GetAsync(int id) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET, id);
            tmp = await ApiClient.PostRequest(tmp);
            UserModel model = JsonConvert.DeserializeObject<UserModel>(tmp);
            return model;
        }

        public async Task<Collection<UserModel>> GetAllAsync() {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.GET_ALL);
            tmp = await ApiClient.PostRequest(tmp);
            Collection<UserModel> model = JsonConvert.DeserializeObject<Collection<UserModel>>(tmp);
            return model;
        }

        public async Task<bool> AddAsync(UserModel model) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.ADD, model);
            tmp = await ApiClient.PostRequest(tmp);
            model.Id = JsonConvert.DeserializeObject<int>(tmp);
            return true;
        }        

        public async Task<bool> DeleteAsync(int id) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.DELETE, id);
            tmp = await ApiClient.PostRequest(tmp);
            bool result = JsonConvert.DeserializeObject<bool>(tmp);
            return result;
        }        

        public bool InsertEmptyMember(UserModel uzivatel) {
            throw new NotImplementedException();
        }

        [Obsolete]
        public async Task<UserModel> Login(string userEmail, string password) {
            throw new NotImplementedException();
        }

        public bool CheckForChanges(int numOfUsers, DateTime lastCheck) {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertBadget(string userEmail, int idOdborka) {
            throw new NotImplementedException();
        }

        //todo body, heslo

        public bool Contains(string email) {
            throw new NotImplementedException();
        }

        public bool BecomeMember(UserModel user) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserModel>> GetAllMembersAsync() {
            throw new NotImplementedException();
        }

        public bool UpdatePassword(string email, string oldPass, string newPass) {
            throw new NotImplementedException();
        }

        public bool Update(UserModel user) {
            throw new NotImplementedException();
        }

        public bool Update(string email, bool isPaid) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserModel>> GetAsync(IEnumerable<int> ids) {
            throw new NotImplementedException();
        }

        [Obsolete]
        public async Task<bool> RegisterAsync(RegistrationRequestModel model) {
            string tmp = jsonFormator.CreateApiRequestString(API.METHOD.REGISTER, model);
            tmp = await ApiClient.PostRequest(tmp);
            int respond = JsonConvert.DeserializeObject<int>(tmp);
            if (respond != -1) {
                model.Id = respond;
                return true;
            }
            return false;
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
