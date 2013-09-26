using Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Domain.Abstract
{
    public interface IAccountRepository
    {
        string GenerateHash(string password, out byte[] salt);
        bool ComparePasswordHash(string value, string hash, byte[] salt);

        void CreateUser(User user);

        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByGuid(object guid);

        void AddUserToRole(string username, string roleName);
        bool IsUserInRole(string username, string roleName);
        void RemoveUserFromRole(string username, string roleName);
        string[] GetRolesForUser(string username);
    }
}
