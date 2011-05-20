using System;
using System.Web.Security;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;
using phiNdus.fundus.Core.Business;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using System.Globalization;

namespace phiNdus.fundus.Core.Web.Security {
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
            this.UserService = IoC.Resolve<IUserService>();
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
            // TODO: Session-Key
            return this.UserService.ChangePassword(null, username, oldPassword, newPassword);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {
            // Todo,jac: Behandlung der verschiednen Fehlerfälle und Status entsprechend setzen.
            status = MembershipCreateStatus.Success;

            return this.ConvertToExternal(
                // TODO: Session-Key
                this.UserService.CreateUser(null ,username, password));
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData) {
            // TODO: Session-Key
            return this.UserService.DeleteUser(null, username);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline) {
            // TODO: Session-Key
            return this.ConvertToExternal(this.UserService.GetUser(null, username));
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
            // TODO: Session-Key
            return this.UserService.ResetPassword(null, username);
        }

        public override void UpdateUser(MembershipUser user) {
            // TODO: Session-Key
            this.UserService.UpdateUser(null, this.ConvertToInternal(user));
        }

        public override bool ValidateUser(string username, string password) {
            // TODO: Session-Key
            return this.UserService.ValidateUser(null, username, password);
        }

        //=========================================================================================
        #region Conversion Helper

        private MembershipUser ConvertToExternal(UserDto userDto) {
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

        private UserDto ConvertToInternal(MembershipUser membershipUser) {
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