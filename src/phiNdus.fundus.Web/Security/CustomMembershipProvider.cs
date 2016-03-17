namespace Phundus.Web.Security
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using Common;
    using Common.Commanding;
    using IdentityAccess.Application;
    using IdentityAccess.Model.Users;
    using IdentityAccess.Projections;
    using IdentityAccess.Users.Services;

    public class CustomMembershipProvider : MembershipProvider
    {
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;

        public IUserQueryService UserQueryService { get; set; }

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
            //Dispatcher.Dispatch(new ChangePassword(username, oldPassword, newPassword));

            return false;
        }

        public bool ChangeEmail(string email, string newEmail)
        {
            //var user = GetUser(email, false);
            //if (user == null)
            //    return false;

            //var userId = user.ProviderUserKey;

            //Dispatcher.Dispatch(new ChangeEmailAddress(Convert.ToInt32(userId), email, newEmail));

            return false;
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

            return user.Account.ValidateKey(key);
        }

        public bool ValidateEmailKey(string key)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = UserQueryService.FindByUsername(username);

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
            AssertionConcern.AssertArgumentNotNull(username, "Username must be provided.");
            AssertionConcern.AssertArgumentNotNull(password, "Password must be provided.");

            username = username.ToLower(CultureInfo.CurrentCulture).Trim();
            var user = Users.FindByEmailAddress(username);

            if (user == null)
                throw new NotFoundException("Ein Benutzer mit dieser E-Mail-Adresse ist uns nicht bekannt.");

            var id = SessionKeyGenerator.CreateKey();
            if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
                id = HttpContext.Current.Session.SessionID;
            user.Account.LogOn(id, password);
            return true;
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

        private MembershipUser ConvertToExternal(UserData user)
        {
            return new MembershipUser(Name, user.EmailAddress,
                new ProviderUserKey(user.UserId).ToString(), user.EmailAddress, null, null,
                user.IsApproved, user.IsLockedOut, user.SignedUpAtUtc, DateTime.MinValue, DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue);
        }
    }
}