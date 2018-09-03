
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway;
using ZalApiGateway.Models;
using ZalApiGateway.Models.ApiCommunicationModels;
using ZalDomain.consts;
using ZalDomain.Models;

namespace ZalDomain.ActiveRecords
{
    public class Badge : IActiveRecord, ISimpleItem
    {
        private BadgeModel model;

        public int Id => model.Id;
        public string Title => model.Name;
        public string Text => model.Text;
        public string Image => model.Image;

        private static BadgeGateway gateway;
        private static BadgeGateway Gateway => gateway ?? (gateway = new BadgeGateway());

        internal static async Task<BaseChangedActiveRecords<Badge>> IfNeededGetAllAsync(ZAL.Rank userRank, DateTime lastCheck) {
            var requestModel = new BadgeRequestModel {
                LastCheck = lastCheck,
                Rank = (int)userRank,
            };
            var rawRespond = await Gateway.IfNeededGetAllAsync(requestModel);
            var badges = rawRespond.Changed.Select(model => new Badge(model));
            return new BaseChangedActiveRecords<Badge>(rawRespond, badges);
        }

        private Badge(BadgeModel model) {
            this.model = model;
        }

        public static async Task<IEnumerable<Badge>> GetAcquiredAsync(User uzivatel) {
            IEnumerable<BadgeModel> rawModels = await Gateway.GetBadgesOwnedByUserAsync(uzivatel.Email, true);
            IEnumerable<Badge> badges = rawModels.Select(model => new Badge(model));
            return badges;
        }

        public static async Task<IEnumerable<Badge>> GetNotAcquired(User uzivatel) {
            IEnumerable<BadgeModel> rawModels = await Gateway.GetBadgesOwnedByUserAsync(uzivatel.Email, false);
            IEnumerable<Badge> badges = rawModels.Select(model => new Badge(model));
            return badges;
        }

        public override string ToString() {
            return model.Name;
        }
    }
}
