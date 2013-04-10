namespace phiNdus.fundus.Web.Security
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.Security;
    using Domain.Repositories;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class FundusMembershipProvider : MembershipProvider
    {
        bool _enablePasswordReset;
        bool _enablePasswordRetrieval;
        int _maxInvalidPasswordAttempts;
        int _minRequiredNonAlphanumericCharacters;
        int _minRequiredPasswordLength;
        int _passwordAttemptWindow;

        public FundusMembershipProvider()
        {
            UserRepositoryFactory = () => GlobalContainer.Resolve<IUserRepository>();
        }

        protected IUserRepository UserRepository
        {
            get { return UserRepositoryFactory(); }
        }

        public override string ApplicationName { get; set; }

        public Func<IUserRepository> UserRepositoryFactory { get; set; }

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
            // 10 Minuten
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //return UserService.ChangePassword(HttpContext.Current.Session.SessionID, username, oldPassword, newPassword);
            throw new NotSupportedException();
        }

        public bool ChangeEmail(string email, string newEmail)
        {
            //return UserService.ChangeEmail(HttpContext.Current.Session.SessionID, email, newEmail);
            throw new NotSupportedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotSupportedException();
        }

        public MembershipUser CreateUser(string email, string password, string firstName, string lastName, int jsNumber,
                                         int? organizationId, out MembershipCreateStatus status)
        {
            //// To Do,jac: Behandlung der verschiednen Fehlerfälle und Status entsprechend setzen.
            //status = MembershipCreateStatus.Success;

            //try
            //{
            //    return ConvertToExternal(
            //        UserService.CreateUser(HttpContext.Current.Session.SessionID, email, password, firstName, lastName,
            //                               jsNumber, organizationId));
            //}
            //catch (EmailAlreadyTakenException)
            //{
            //    status = MembershipCreateStatus.DuplicateEmail;
            //    return null;
            //}
            throw new NotSupportedException();
        }

        public bool ValidateValidationKey(string key)
        {
            //return UserService.ValidateValidationKey(key);
            throw new NotSupportedException();
        }

        public bool ValidateEmailKey(string key)
        {
            //return UserService.ValidateEmailKey(key);
            throw new NotSupportedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            //return ConvertToExternal(UserService.GetUser(HttpContext.Current.Session.SessionID, username));
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            //return email;
            throw new NotSupportedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            //return UserService.ResetPassword(HttpContext.Current.Session.SessionID, username);
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            //UserService.UpdateUser(HttpContext.Current.Session.SessionID, ConvertToInternal(user));
            throw new NotSupportedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            //return UserService.ValidateUser(HttpContext.Current.Session.SessionID, username, password);
            throw new NotSupportedException();
        }


        //static MembershipUser ConvertToExternal(UserDto userDto)
        //{
        //    return new MembershipUser(
        //        Membership.Provider.Name,
        //        userDto.Email,
        //        userDto,
        //        userDto.Email,
        //        null,
        //        null,
        //        userDto.IsApproved,
        //        false,
        //        userDto.CreateDate,
        //        DateTime.Now,
        //        DateTime.Now,
        //        DateTime.Now,
        //        DateTime.Now);
        //}

        //static UserDto ConvertToInternal(MembershipUser membershipUser)
        //{
        //    return new UserDto
        //        {
        //            Email = membershipUser.UserName,
        //            IsApproved = membershipUser.IsApproved,
        //            CreateDate = membershipUser.CreationDate
        //        };
        //}


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
    }
}