using Ninject;
using Site.Domain.Abstract;
using Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CustomMembershipExample.Infrastructure
{
    public class CustomMembershipProvider : MembershipProvider
    {
        [Inject]
        public IAccountRepository accountRepository { get; set; }

        #region Initial Values

        private string applicationName = "CustomMembershipExample";
        private bool enablePasswordReset = true;
        private bool enablePasswordRetrieval = false;
        private bool requiresQuestionAndAnswer = false;
        private bool requiresUniqueEmail = true;
        private int maxInvalidPasswordAttempts = 5;
        private int passwordAttemptWindow = 10;
        private int minRequiredPasswordLength = 8;

        #endregion

        #region Assignments

        public override string ApplicationName
        {
            get { return applicationName; }
            set { value = applicationName; }
        }

        public override int PasswordAttemptWindow
        {
            get { return passwordAttemptWindow; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return requiresUniqueEmail; }
        }

        public override bool EnablePasswordReset
        {
            get { return enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }

        #endregion

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if ((RequiresUniqueEmail && (GetUserNameByEmail(email) != String.Empty)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser membershipUser = GetUser(username, false);

            if (membershipUser == null)
            {
                byte[] salt;

                User user = new User();
                user.UserGuid = Guid.NewGuid().ToString();
                user.Username = username;
                user.PasswordHash = accountRepository.GenerateHash(password, out salt);
                user.PasswordSalt = salt;
                user.Email = email;
                user.SignUpDate = DateTime.Now;
                user.LastLoginDate = DateTime.Now;
                user.LastActiveTime = DateTime.Now;

                accountRepository.CreateUser(user);

                status = MembershipCreateStatus.Success;
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        public override bool ValidateUser(string username, string password)
        {
            User user = accountRepository.GetUserByUsername(username);

            if (user != null)
                return accountRepository.ComparePasswordHash(password, user.PasswordHash, user.PasswordSalt);
            else
                return false;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            User user = accountRepository.GetUserByUsername(username);

            if (user != null)
            {
                MembershipUser membershipUser = new MembershipUser("SiteMembershipProvider", user.Username, user.UserGuid, user.Email,
                                                                    null, null, true, false, user.SignUpDate, user.LastLoginDate, user.LastActiveTime,
                                                                    DateTime.MinValue, DateTime.MinValue);

                return membershipUser;
            }
            else
            {
                return null;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            User user = accountRepository.GetUserByGuid(providerUserKey);

            if (user != null)
            {
                MembershipUser membershipUser = new MembershipUser("SiteMembershipProvider", user.Username, user.UserGuid, user.Email,
                                                                    null, null, true, false, user.SignUpDate, user.LastLoginDate, user.LastActiveTime,
                                                                    DateTime.MinValue, DateTime.MinValue);

                return membershipUser;
            }
            else
            {
                return null;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            var user = accountRepository.GetUserByEmail(email);

            if (user != null)
                return user.Username;
            else
                return String.Empty;
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #region Unused

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}