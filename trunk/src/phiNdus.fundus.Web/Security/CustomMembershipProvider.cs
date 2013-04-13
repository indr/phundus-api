namespace phiNdus.fundus.Web.Security
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using Business;
    using Domain;
    using Domain.Infrastructure;
    using Domain.Mails;
    using Domain.Repositories;
    using fundus.Business;

    public class CustomMembershipProvider : MembershipProvider
    {
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;

        public IUserRepository Users { get; set; }

        public override string ApplicationName { get; set; }

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
            var user = Users.FindByEmail(username);
            user.Membership.ChangePassword(oldPassword, newPassword);
            return true;
        }

        public bool ChangeEmail(string email, string newEmail)
        {
            email = email.ToLower(CultureInfo.CurrentCulture).Trim();
            newEmail = newEmail.ToLower(CultureInfo.CurrentCulture).Trim();

            // TODO: In User.ChangeEmail()-Methode verschieben. Teffig bitte!
            if (Users.FindByEmail(newEmail) != null)
                throw new EmailAlreadyTakenException();

            var user = Users.FindByEmail(email);
            user.Membership.RequestedEmail = newEmail;
            user.Membership.GenerateValidationKey();
            Users.Update(user);
            new UserChangeEmailValidationMail().For(user).Send(user);

            return true;
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
            var user = Users.FindByValidationKey(key);
            if (user == null)
                return false;

            var result = user.Membership.ValidateValidationKey(key);
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
            if (Users.FindByEmail(user.Membership.RequestedEmail) != null)
                throw new EmailAlreadyTakenException();

            var result = user.Membership.ValidateEmailKey(key);
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
            //return ConvertToExternal(UserService.GetUser(HttpContext.Current.Session.SessionID, username));
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            return email;
        }

        public override string ResetPassword(string username, string answer)
        {
            var user = Users.FindByEmail(username);
            if (user == null)
                throw new Exception("Die E-Mail-Adresse konnte nicht gefunden werden.");
            var password = user.Membership.ResetPassword();
            Users.Update(user);
            new UserResetPasswordMail().For(user, password).Send(user);
            return password;
        }

        public override void UpdateUser(MembershipUser user)
        {
            //UserService.UpdateUser(HttpContext.Current.Session.SessionID, ConvertToInternal(user));
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
                user.Membership.LogOn(id, password);
                if (user.SelectedOrganization == null && user.Memberships.Count > 0)
                    user.SelectOrganization(user.Memberships.First().Organization);
                if (user.SelectedOrganization != null)
                    HttpContext.Current.Session["OrganizationId"] = user.SelectedOrganization.Id;
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
    }
}