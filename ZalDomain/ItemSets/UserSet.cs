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
        private Collection<User> Users;
        private DateTime LastCheck;

        public UserSet() {
            Users = new Collection<User>();
            LastCheck = ZAL.DATE_OF_ORIGIN;
        }

        public User Login(string email, string password) {
            return User.Login(email, password);
        }

        public void Synchronize() {
            if (AnyChanges()) {
                Users = User.GetAllMembers();
                LastCheck = DateTime.Now;
            }
        }

        internal Collection<User> GetByEmailList(List<string> list) {
            Collection<User> users = new Collection<User>();
            foreach(string email in list) {
                users.Add(GetByEmail(email));
            }
            return users;
        }

        internal void ReSynchronize() {
            LastCheck = ZAL.DATE_OF_ORIGIN;
            Synchronize();
        }

        private bool AnyChanges() {
            return User.CheckForChanges(Users.Count, LastCheck);
        }

        public User RegisterNewUser(string name, string surname, string phone, string email, string password) {
            User user = User.RegisterNew(email, name, surname, phone, password);
            Users.Add(user);
            return user;
        }

        public void AddNewEmptyUser(string name, string surname, int group) {
            IntegrityCondition.UserIsLeader();
            Users.Add(User.AddNewEmptyUser(name, surname, group));
        }

        public User GetByEmail(string email) {
            foreach (User u in Users) {
                if (u.Has(CONST.USER.EMAIL, email)) {
                    return u;
                }
            }
            User user = User.Select(email);
            if (user.Email != null) {
                Users.Add(user);
                return user;
            }
            return User.Empty();
        }

        public Collection<User> GetAll() {
            Synchronize();
            return Users;
        }

        public bool Contains(string email) {
            return User.Contains(email);
        }

        public Collection<User> GetBy(int key, int value) {
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
        }

        //public Collection<User> GetNonMembers(int)

        internal User Select(string email) {
            return User.Select(email);
        }
    }
}
