using Site.Domain.Abstract;
using Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Site.Domain.Concrete
{
    public class SqlAccountRepository : IAccountRepository
    {
        private DataContext dataContext;
        private Table<User> usersTable;
        private Table<Role> rolesTable;
        private Table<UsersInRoles> usersInRolesTable;

        public SqlAccountRepository(string connectionString)
        {
            dataContext = new DataContext(connectionString);

            usersTable = dataContext.GetTable<User>();
            rolesTable = dataContext.GetTable<Role>();
            usersInRolesTable = dataContext.GetTable<UsersInRoles>();
        }

        #region Users

        public string GenerateHash(string password, out byte[] salt)
        {
            SHA256 sha = new SHA256Managed();

            byte[] random = new Byte[100];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);
            salt = random;

            password = password + salt;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] result;

            result = sha.ComputeHash(data);

            return BitConverter.ToString(result).Replace("-", "");
        }

        public bool ComparePasswordHash(string value, string hash, byte[] salt)
        {
            SHA256 sha = new SHA256Managed();
            value = value + salt;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] result;

            result = sha.ComputeHash(data);

            value = BitConverter.ToString(result).Replace("-", "");

            if (value != hash)
                return false;
            else
                return true;
        }

        public void CreateUser(User user)
        {
            usersTable.InsertOnSubmit(user);
            usersTable.Context.SubmitChanges();
        }

        public User GetUserByUsername(string username)
        {
            return usersTable.FirstOrDefault(x => x.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return usersTable.FirstOrDefault(x => x.Email == email);
        }

        public User GetUserByGuid(object guid)
        {
            return usersTable.FirstOrDefault(x => x.UserGuid == guid);
        }

        #endregion

        #region Roles

        public void AddUserToRole(string username, string roleName)
        {
            var user = usersTable.FirstOrDefault(x => x.Username == username);
            var role = rolesTable.FirstOrDefault(x => x.RoleName == roleName);

            var userInRole = usersInRolesTable.FirstOrDefault(x => x.UserId == user.UserId);

            if (userInRole == null)
            {
                UsersInRoles ur = new UsersInRoles();
                ur.RoleId = role.RoleId;
                ur.UserId = user.UserId;

                usersInRolesTable.InsertOnSubmit(ur);
            }
            else
            {
                userInRole.RoleId = role.RoleId;
                usersInRolesTable.Context.Refresh(RefreshMode.KeepChanges, userInRole);
            }

            usersInRolesTable.Context.SubmitChanges();
        }

        public bool IsUserInRole(string username, string roleName)
        {
            var user = usersTable.FirstOrDefault(x => x.Username == username);
            var role = rolesTable.FirstOrDefault(x => x.RoleName == roleName);

            var userInRole = usersInRolesTable.FirstOrDefault(x => x.UserId == user.UserId && x.RoleId == role.RoleId);

            if (userInRole != null)
                return true;
            else
                return false;
        }

        public void RemoveUserFromRole(string username, string roleName)
        {
            var user = usersTable.FirstOrDefault(x => x.Username == username);
            var role = rolesTable.FirstOrDefault(x => x.RoleName == roleName);

            var ur = usersInRolesTable.FirstOrDefault(x => x.UserId == user.UserId && x.RoleId == role.RoleId);

            ur.RoleId = 0;

            usersInRolesTable.Context.Refresh(RefreshMode.KeepChanges, ur);
            usersInRolesTable.Context.SubmitChanges();
        }

        public string[] GetRolesForUser(string username)
        {
            IList<string> roleNames = new List<string>();

            var user = usersTable.FirstOrDefault(x => x.Username == username);

            var userInRoles = usersInRolesTable.Where(x => x.UserId == user.UserId).ToList();

            if (userInRoles.Count > 0)
            {
                foreach (var r in userInRoles)
                {
                    roleNames.Add(r.Role.RoleName);
                }
            }

            return roleNames.ToArray();
        }

        #endregion
    }
}
