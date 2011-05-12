using System;
using System.Web.Security;
using Rhino.Commons;
using phiNdus.fundus.Core.Business;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Web.Security {
    public class FundusMembershipProvider : MembershipProvider {

        //=========================================================================================
        #region Configuration

        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private int maxInvalidPasswordAttempts;
        private int minRequiredPasswordLength;
        private int minRequiredNonAlphanumericCharacters;
        private int passwordAttemptWindow;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config) {
            base.Initialize(name, config);
            enablePasswordReset = bool.Parse(config["enablePasswordReset"] ?? "false");
            enablePasswordRetrieval = bool.Parse(config["enablePasswordRetrieval"] ?? "false");
            ApplicationName = config["applicationName"];
            maxInvalidPasswordAttempts = Int32.Parse(config["maxInvalidPasswordAttempts"] ?? "5");
            minRequiredPasswordLength = Int32.Parse(config["minRequiredPasswordLength"] ?? "8");
            minRequiredNonAlphanumericCharacters = Int32.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "2");
            passwordAttemptWindow = Int32.Parse(config["passwordAttemptWindow"] ?? "10"); // 10 Minuten
        }
        #endregion
        //=========================================================================================

        public FundusMembershipProvider() {
            // TODO,chris UserService von IoC erstellen
            this._userService = IoC.Resolve<IUserService>();

            int bla = 5;
        }

        private IUserService _userService { get; set; }

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset {
            get { return enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval {
            get { return enablePasswordRetrieval; }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword) {
            return this._userService.ChangePassword(username, oldPassword, newPassword);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {
            // Todo,jac: Behandlung der verschiednen Fehlerfälle und Status entsprechend setzen.
            status = MembershipCreateStatus.Success;

            return this.ConvertToExternal(
                this._userService.CreateUser(username, password, passwordQuestion, passwordAnswer));
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData) {
            return this._userService.DeleteUser(username);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline) {
            return this.ConvertToExternal(this._userService.GetUser(username));
        }

        public override string GetUserNameByEmail(string email) {
            return email;
        }

        public override int MaxInvalidPasswordAttempts {
            get { return maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters {
            get { return minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength {
            get { return minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow {
            get { return passwordAttemptWindow; }
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
            return this._userService.ResetPassword(username);
        }

        public override void UpdateUser(MembershipUser user) {
            this._userService.UpdateUser(this.ConvertToInternal(user));
        }

        public override bool ValidateUser(string username, string password) {
            return this._userService.ValidateUser(username, password);
        }

        //=========================================================================================
        #region Conversion Helper

        private MembershipUser ConvertToExternal(UserDto userDto) {
            return new MembershipUser(
                Membership.Provider.Name,
                userDto.Email,
                userDto,
                userDto.Email,
                userDto.PasswordQuestion,
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