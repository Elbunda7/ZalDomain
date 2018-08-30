using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System;
using ZalApiGateway.ApiTools;
using ZalApiGateway.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZalApiGateway
{
    public class BadgeGateway:Gateway
    {
        public BadgeGateway() : base(API.ENDPOINT.BADGES) { }

        public async Task<BadgeModel> GetAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BadgeModel>> GetAllAsync() {
            return SendRequestFor<IEnumerable<BadgeModel>>(API.METHOD.GET_ALL);
        }

        public async Task<Collection<BadgeModel>> GetBadgesOwnedByUserAsync(string userEmail, bool haveThem) {
            throw new NotImplementedException();
        }

        public async Task<bool> GiveBadgeToUser(int idOdborka, string userEmail) {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveBadgeToUser(int idOdborka, string userEmail) {
            throw new NotImplementedException();
        }

    }
}
