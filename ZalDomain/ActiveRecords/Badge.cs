
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalApiGateway;
using ZalApiGateway.Models;

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

        public static async Task<IEnumerable<Badge>> GetAllAsync() {
            IEnumerable<BadgeModel> rawModels = await Gateway.GetAllAsync();
            IEnumerable<Badge> badges = rawModels.Select(model => new Badge(model));
            return badges;
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
