using Ninject;
using Site.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CustomMembershipExample.Infrastructure
{
    public class CustomRoleProvider : RoleProvider
    {
        [Inject]
        public IAccountRepository accountRepository { get; set; }

        #region Assignments

        public override string ApplicationName
        {
            get
            {
                return "Social";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var u in usernames)
            {
                foreach (var r in roleNames)
                {
                    accountRepository.AddUserToRole(u, r);
                }
            }
        }

        public void AddUsersToRoles(string username, string roleName)
        {
            accountRepository.AddUserToRole(username, roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return accountRepository.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var u in usernames)
            {
                foreach (var r in roleNames)
                {
                    accountRepository.RemoveUserFromRole(u, r);
                }
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            return accountRepository.GetRolesForUser(username);
        }

        #region Not Used

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}