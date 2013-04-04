using System;
using System.Web;
using System.Web.Security;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Business;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Services;
using System.Globalization;

namespace phiNdus.fundus.Web.Security {
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class FundusMembershipProvider : MembershipProvider {

        //=========================================================================================
        #region Configuration

        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredPasswordLength;
        private int _minRequiredNonAlphanumericCharacters;
        private int _passwordAttemptWindow;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config) {
            base.Initialize(name, config);

            this._enablePasswordReset = bool.Parse(config["enablePasswordReset"] ?? "false");
            this._enablePasswordRetrieval = bool.Parse(config["enablePasswordRetrieval"] ?? "false");
            this.ApplicationName = config["applicationName"];
            this._maxInvalidPasswordAttempts = int.Parse(config["maxInvalidPasswordAttempts"] ?? "5", CultureInfo.InvariantCulture);
            this._minRequiredPasswordLength = int.Parse(config["minRequiredPasswordLength"] ?? "8", CultureInfo.InvariantCulture);
            this._minRequiredNonAlphanumericCharacters = int.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "2", CultureInfo.InvariantCulture);
            this._passwordAttemptWindow = int.Parse(config["passwordAttemptWindow"] ?? "10", CultureInfo.InvariantCulture); // 10 Minuten
        }
        #endregion
        //=========================================================================================

        public FundusMembershipProvider() {
            this.UserService = GlobalContainer.Resolve<IUserService>();
        }

        private IUserService UserService { get; set; }

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset {
            get { return this._enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval {
            get { return this._enablePasswordRetrieval; }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword) {
            return this.UserService.ChangePassword(HttpContext.Current.Session.SessionID, username, oldPassword, newPassword);
        }

        public bool ChangeEmail(string email, string newEmail)
        {
            return this.UserService.ChangeEmail(HttpContext.Current.Session.SessionID, email, newEmail);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021", MessageId = "Avoid out parameters",
            Justification = "Kann nicht geändert werden, da vom Framework so vorgegeben.")]
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {
            throw new InvalidOperationException();
        }

        public MembershipUser CreateUser(string email, string password, string firstName, string lastName, int jsNumber, int? organizationId, out MembershipCreateStatus status)
        {
            // Todo,jac: Behandlung der verschiednen Fehlerfälle und Status entsprechend setzen.
            status = MembershipCreateStatus.Success;

            try
            {
                return ConvertToExternal(
                    this.UserService.CreateUser(HttpContext.Current.Session.SessionID, email, password, firstName, lastName, jsNumber, organizationId));
            }
            catch (EmailAlreadyTakenException)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
        }

        public bool ValidateValidationKey(string key)
        {
            return this.UserService.ValidateValidationKey(key);
        }

        public bool ValidateEmailKey(string key)
        {
            return this.UserService.ValidateEmailKey(key);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData) {
            return this.UserService.DeleteUser(HttpContext.Current.Session.SessionID, username);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline) {
            return ConvertToExternal(this.UserService.GetUser(HttpContext.Current.Session.SessionID, username));
        }

        public override string GetUserNameByEmail(string email) {
            return email;
        }

        public override int MaxInvalidPasswordAttempts {
            get { return this._maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters {
            get { return this._minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength {
            get { return this._minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow {
            get { return this._passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression {
            get { return null; }
        }

        public override bool RequiresQuestionAndAnswer {
            get { return false; }
        }

        public override bool RequiresUniqueEmail {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer) {
            return this.UserService.ResetPassword(HttpContext.Current.Session.SessionID, username);
        }

        public override void UpdateUser(MembershipUser user) {
            this.UserService.UpdateUser(HttpContext.Current.Session.SessionID, ConvertToInternal(user));
        }

        public override bool ValidateUser(string username, string password)
        {
            // ASP.NET-Bug? Session-Id ändert sich, falls nichts in der Session ist.
            HttpContext.Current.Session["dummy"] = new object();
            return this.UserService.ValidateUser(HttpContext.Current.Session.SessionID, username, password);
        }

        //=========================================================================================
        #region Conversion Helper

        private static MembershipUser ConvertToExternal(UserDto userDto) {
            return new MembershipUser(
                Membership.Provider.Name,
                userDto.Email,
                userDto,
                userDto.Email,
                null,
                null,
                userDto.IsApproved,
                false,
                userDto.CreateDate,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now);
        }

        private static UserDto ConvertToInternal(MembershipUser membershipUser) {
            return new UserDto {
                Email = membershipUser.UserName,
                IsApproved = membershipUser.IsApproved,
                CreateDate = membershipUser.CreationDate
            };
        }

        #endregion
        //=========================================================================================

        //=========================================================================================
        #region not yet implemented

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline) {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline() {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer) {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName) {
            throw new NotImplementedException();
        }

        #endregion
        //=========================================================================================
        
    }


}