namespace phiNdus.fundus.Web.Security
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using Phundus.Core.Cqrs;
    using Phundus.Core.IdentityAndAccess.Queries;
    using Phundus.Core.IdentityAndAccess.Users.Commands;
    using Phundus.Core.IdentityAndAccess.Users.Exceptions;
    using Phundus.Core.IdentityAndAccess.Users.Mails;
    using Phundus.Core.IdentityAndAccess.Users.Repositories;
    using Phundus.Core.IdentityAndAccess.Users.Services;
    using Phundus.Infrastructure;

    public class CustomMembershipProvider : MembershipProvider
    {
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;

        public IUserQueries UserQueries { get; set; }

        public IUserRepository Users { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        public override string ApplicationName { get; set; }

        public override string Name
        {
            get { return "CustomProvider"; }
        }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return null; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            _enablePasswordReset = bool.Parse(config["enablePasswordReset"] ?? "false");
            _enablePasswordRetrieval = bool.Parse(config["enablePasswordRetrieval"] ?? "false");
            ApplicationName = config["applicationName"];
            _maxInvalidPasswordAttempts = int.Parse(config["maxInvalidPasswordAttempts"] ?? "5",
                CultureInfo.InvariantCulture);
            _minRequiredPasswordLength = int.Parse(config["minRequiredPasswordLength"] ?? "8",
                CultureInfo.InvariantCulture);
            _minRequiredNonAlphanumericCharacters = int.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "2",
                CultureInfo.InvariantCulture);
            _passwordAttemptWindow = int.Parse(config["passwordAttemptWindow"] ?? "10", CultureInfo.InvariantCulture);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            Dispatcher.Dispatch(new ChangePassword(username, oldPassword, newPassword));

            return true;
        }

        public bool ChangeEmail(string email, string newEmail)
        {
            var user = GetUser(email, false);
            if (user == null)
                return false;

            var userId = user.ProviderUserKey;

            Dispatcher.Dispatch(new ChangeEmailAddress(Convert.ToInt32(userId), email, newEmail));

            return true;
        }

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved,
            object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotSupportedException();
        }

        public bool ValidateValidationKey(string key)
        {
            var user = Users.FindByValidationKey(key);
            if (user == null)
                return false;

            var result = user.Account.ValidateValidationKey(key);
            if (result)
            {
                Users.Update(user);
                new UserAccountCreatedMail().For(user).Send(Config.FeedbackRecipients);
            }
            return result;
        }

        public bool ValidateEmailKey(string key)
        {
            var user = Users.FindByValidationKey(key);
            if (user == null)
                return false;

            // Prüfen ob Benutzer bereits exisitiert.
            if (Users.FindByEmail(user.Account.RequestedEmail) != null)
                throw new EmailAlreadyTakenException();

            var result = user.Account.ValidateEmailKey(key);
            if (result)
            {
                Users.Update(user);
                //new UserAccountCreatedMail().For(user).Send(Settings.Common.AdminEmailAddress);
            }
            return result;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = UserQueries.ByUserName(username);

            if (user == null)
                return null;
            return ConvertToExternal(user);
        }

        public override string GetUserNameByEmail(string email)
        {
            return email.ToLowerInvariant();
        }

        public override string ResetPassword(string username, string answer)
        {
            Dispatcher.Dispatch(new ResetPassword(username));

            return null;
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            username = username.ToLower(CultureInfo.CurrentCulture).Trim();
            var user = Users.FindByEmail(username);

            if (user == null)
                return false;

            try
            {
                var id = SessionKeyGenerator.CreateKey();
                if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
                    id = HttpContext.Current.Session.SessionID;
                user.Account.LogOn(id, password);
                return true;
            }
            catch (InvalidPasswordException)
            {
                return false;
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotSupportedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        private MembershipUser ConvertToExternal(UserDto user)
        {
            return new MembershipUser(Name, user.Email, user.Id, user.Email, null, null, user.IsApproved,
                user.IsLockedOut, user.CreateDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue,
                DateTime.MinValue);
        }
    }
}