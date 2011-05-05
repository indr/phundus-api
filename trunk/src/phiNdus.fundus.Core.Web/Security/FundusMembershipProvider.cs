using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using phiNdus.fundus.Core.Business;

namespace phiNdus.fundus.Core.Web.Security {
    public class FundusMembershipProvider : MembershipProvider {

        public FundusMembershipProvider(IUserService userService) {
            this.UserService = userService;
        }

        private IUserService UserService { get; set; }

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval {
            get { return false; }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword) {
            return this.UserService.ChangePassword(username, oldPassword, newPassword);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {
            // Todo,jac: Behandlung der verschiednen Fehlerfälle und Status entsprechend setzen.
            status = MembershipCreateStatus.Success;

            return this.ConvertToExternal(
                this.UserService.CreateUser(username, password, passwordQuestion, passwordAnswer, isApproved));
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData) {
            return this.UserService.DeleteUser(username);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline) {
            return this.ConvertToExternal(this.UserService.GetUser(username));
        }

        public override string GetUserNameByEmail(string email) {
            return email;
        }

        public override int MaxInvalidPasswordAttempts {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters {
            get { return 2; }
        }

        public override int MinRequiredPasswordLength {
            get { return 8; }
        }

        public override int PasswordAttemptWindow {
            get { return 10; /* Minuten */ }
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
            return this.UserService.ResetPassword(username);
        }

        public override void UpdateUser(MembershipUser user) {
            this.UserService.UpdateUser(this.ConvertToInternal(user));
        }

        public override bool ValidateUser(string username, string password) {
            return this.UserService.ValidateUser(username, password);
        }

        //=========================================================================================
        #region Conversion Helper

        private MembershipUser ConvertToExternal(UserDto userDto) {
            return new MembershipUser(
                Membership.Provider.Name,
                userDto.Mail,
                userDto,
                userDto.Mail,
                userDto.PasswordQuestion,
                null,
                userDto.Approved,
                false,
                userDto.CreationDate,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now);
        }

        private UserDto ConvertToInternal(MembershipUser membershipUser) {
            return new UserDto {
                Mail = membershipUser.UserName,
                Approved = membershipUser.IsApproved,
                CreationDate = membershipUser.CreationDate
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