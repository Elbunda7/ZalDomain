using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalDomain.ActiveRecords;
using ZalDomain.consts;
using ZalDomain.tools;
using ZalDomain.tools.ARSets;
using static ZalDomain.consts.ZAL;

namespace ZalDomain.ItemSets
{
    public class UserSet
    {
        public UserObservableSortedSet Users { get; set; }

        public UserSet() {
            Users = new UserObservableSortedSet();
        }

        public async Task ReSynchronize(Group groups = Group.AllClub, Rank ranks = Rank.All, UserRole roles = UserRole.All) {
            Users.LastSynchronization = DATE_OF_ORIGIN;
            await SynchronizeUsers(groups, ranks, roles);
        }

        public async Task SynchronizeUsers(Group groups = Group.AllClub, Rank ranks = Rank.All, UserRole roles = UserRole.All) {//todo kontrola korektnosti při změně filtru
            if (Users.LastSynchronization == DATE_OF_ORIGIN || true) {//todo LoadChanges
                await LoadUsers(groups, ranks, roles);
            }
            else {
                await LoadUserChanges(groups, ranks, roles);
            }
        }

        private async Task LoadUsers(Group groups, Rank ranks, UserRole roles) {
            var respond = await User.GetAll(groups, ranks, roles);
            Users.AddOrUpdateAll(respond.ActiveRecords);
            Users.LastSynchronization = respond.Timestamp;
        }

        private async Task LoadUserChanges(Group groups, Rank ranks, UserRole roles) {
            throw new NotImplementedException();
        }

        

        //internal Collection<User> GetByEmailList(List<string> list) {
        //    Collection<User> users = new Collection<User>();
        //    foreach(string email in list) {
        //        users.Add(GetByEmail(email));
        //    }
        //    return users;
        //}

        private bool AnyChanges() {
            throw new NotImplementedException();
            //return User.CheckForChanges(Users.Count, LastCheck);
        }

        public async Task AddNewEmptyUser(string name, string surname, int group) {
            UserPermision.HasRank(Zal.Session.CurrentUser, ZAL.Rank.Vedouci);
            Users.Add(await User.AddNewEmptyUser(name, surname, group));
        }

        //public User GetByEmail(string email) {
        //    foreach (User u in Users) {
        //        if (u.Has(CONST.USER.EMAIL, email)) {
        //            return u;
        //        }
        //    }
        //    User user = User.Select(email);
        //    if (user.Email != null) {
        //        Users.Add(user);
        //        return user;
        //    }
        //    return User.Empty();
        //}

        internal async Task<User> Get(int id) {
            //return User.Select(email);

            User a = Users.Single(user => user.Id == id);
            if (a == null) {
                a = await User.GetAsync(id);
                Users.Add(a);
            }
            return a;
        }

        internal async Task<IEnumerable<User>> Get(List<int> ids) {
            IEnumerable<User> users = Users.Where(user => ids.Any(id => id == user.Id));
            var notLoadedIds = ids.Where(id => users.All(user => user.Id != id));
            users.Union(await User.GetAsync(notLoadedIds));
            return users;
        }
    }
}
