using ZalDomain.ActiveRecords;
using ZalDomain.consts;
using ZalDomain.tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.ItemSets
{
    public class UserSet
    {
        private Collection<User> Data;
        private DateTime LastCheck;

        public UserSet() {
            Data = new Collection<User>();
            LastCheck = ZAL.DATE_OF_ORIGIN;
        }

        public async Task<User> LoginAsync(string email, string password) {
            return await User.Login(email, password);
        }

        public async void Synchronize() {
            if (AnyChanges()) {
                Data = await User.GetAllMembers() as Collection<User>;
                LastCheck = DateTime.Now;
            }
        }

        //internal Collection<User> GetByEmailList(List<string> list) {
        //    Collection<User> users = new Collection<User>();
        //    foreach(string email in list) {
        //        users.Add(GetByEmail(email));
        //    }
        //    return users;
        //}

        internal void ReSynchronize() {
            LastCheck = ZAL.DATE_OF_ORIGIN;
            Synchronize();
        }

        private bool AnyChanges() {
            return User.CheckForChanges(Data.Count, LastCheck);
        }

        public async Task<User> RegisterNewUserAsync(string name, string surname, string phone, string email, string password) {
            User user = await User.RegisterNewAsync(email, name, surname, phone, password);
            Data.Add(user);
            return user;
        }

        public void AddNewEmptyUser(string name, string surname, int group) {
            UserPermision.HasRank(Zal.Session.CurrentUser, ZAL.RANK.VEDOUCI);
            Data.Add(User.AddNewEmptyUser(name, surname, group));
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

        public Collection<User> GetAll() {
            Synchronize();
            return Data;
        }

        public bool Contains(string email) {
            return User.Contains(email);
        }

        /*public Collection<User> GetBy(int key, int value) {
            Synchronize();
            Collection<User> tmp = new Collection<User>();
            foreach (User u in Users) {
                if (u.Has(key, value)) {
                    tmp.Add(u);
                }
            }
            return tmp;
        }

        public Collection<User> GetBy(int key, String value) {
            Synchronize();
            Collection<User> tmp = new Collection<User>();
            foreach (User u in Users) {
                if (u.Has(key, value)) {
                    tmp.Add(u);
                }
            }
            return tmp;
        }

        public Collection<User> GetByGroup(int group) {
            Synchronize();
            Collection<User> tmp = new Collection<User>();
            foreach (User u in Users) {
                if (u.Has(CONST.USER.DRUZINA, group)) {
                    tmp.Add(u);
                }
            }
            return tmp;
        }*/

        //public Collection<User> GetNonMembers(int)

        internal async Task<User> Get(int id) {
            //return User.Select(email);

            User a = Data.Single(user => user.Id == id);
            if (a == null) {
                a = await User.GetAsync(id);
                Data.Add(a);
            }
            return a;
        }

        internal async Task<IEnumerable<User>> Get(List<int> ids) {
            IEnumerable<User> users = Data.Where(user => ids.Any(id => id == user.Id));
            var notLoadedIds = ids.Where(id => users.All(user => user.Id != id));
            users.Union(await User.GetAsync(notLoadedIds));
            return users;
        }
    }
}
