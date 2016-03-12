namespace Phundus.IdentityAccess.Model.Users
{
    using System;
    using Common.Domain.Model;
    using Common.Eventing;
    using IdentityAccess.Users.Exceptions;
    using IdentityAccess.Users.Model;
    using IdentityAccess.Users.Services;

    public class Account : EntityBase
    {
        private DateTime _createDate;
        private string _password;
        private string _salt;

        public Account()
        {
            _createDate = DateTime.UtcNow;
            _salt = KeyGenerator.CreateKey(5);
        }

        public virtual User User { get; set; }

        public virtual string Password
        {
            get { return _password; }
        }

        public virtual string Salt
        {
            get { return _salt; }
            protected set { _salt = value; }
        }

        public virtual string Email { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }

        public virtual string SessionKey { get; protected set; }
        public virtual DateTime? LastLogOnDate { get; set; }
        public virtual DateTime? LastPasswordChangeDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual string Comment { get; set; }

        protected virtual bool IsNotApproved
        {
            get { return !IsApproved; }
            set { IsApproved = !value; }
        }

        public virtual string ValidationKey { get; protected set; }

        public virtual string RequestedEmail { get; set; }

        public virtual void Lock(Initiator initiator)
        {
            if (IsLockedOut)
                return;

            IsLockedOut = true;
            LastLockoutDate = DateTime.UtcNow;

            EventPublisher.Publish(new UserLocked(initiator, User.UserId, LastLockoutDate.Value));
        }

        public virtual void Unlock(Initiator initiator)
        {
            if (!IsLockedOut)
                return;

            IsLockedOut = false;

            EventPublisher.Publish(new UserUnlocked(initiator, User.UserId, LastLockoutDate.GetValueOrDefault()));
        }

        public virtual void LogOn(string sessionKey, string password)
        {
            Guard.Against<ArgumentNullException>(sessionKey == null, "sessionKey");
            Guard.Against<ArgumentException>(String.IsNullOrEmpty(sessionKey), "sessionKey must not be empty");
            Guard.Against<ArgumentNullException>(password == null, "password");
            Guard.Against<UserNotApprovedException>(IsNotApproved, "Dieser Benutzer hat seine E-Mail-Adresse noch nicht bestätigt.");
            Guard.Against<UserLockedOutException>(IsLockedOut, "Dieser Benutzer wurde von uns gesperrt.");

            Guard.Against<InvalidPasswordException>(
                Password != PasswordEncryptor.Encrypt(password, Salt), "Das angegebene Passwort ist falsch.");

            SessionKey = sessionKey;
            LastLogOnDate = DateTime.Now;

            EventPublisher.Publish(new UserLoggedIn(User.Id));
        }


        public virtual void ChangeEmailAddress(Initiator initiator, string password, string newEmailAddress)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");

            if (PasswordEncryptor.Encrypt(password, Salt) != Password)
                throw new InvalidPasswordException();

            GenerateValidationKey();
            RequestedEmail = newEmailAddress;

            EventPublisher.Publish(new EmailAddressChangeRequested(initiator, User.UserId, User.FirstName, User.LastName, RequestedEmail, ValidationKey));
        }

        public virtual void ChangePassword(string oldPassword, string newPassword)
        {
            if (oldPassword == null) throw new ArgumentNullException("oldPassword");
            if (newPassword == null) throw new ArgumentNullException("newPassword");


            Guard.Against<InvalidPasswordException>(
                Password != PasswordEncryptor.Encrypt(oldPassword, Salt), "Das alte Passwort ist falsch.");

            SetPassword(newPassword);
        }

        public virtual string ResetPassword()
        {
            var newPassword = PasswordGenerator.CreatePassword();
            SetPassword(newPassword);
            return newPassword;
        }

        public virtual string GenerateValidationKey()
        {
            var key = KeyGenerator.CreateKey(24);
            ValidationKey = key;
            return key;
        }

        public virtual bool ValidateKey(string key)
        {
            if (key != ValidationKey)
                return false;

            IsApproved = true;
            ValidationKey = null;
            if (RequestedEmail != null)
            {
                var oldEmailAddress = Email;
                Email = RequestedEmail;
                RequestedEmail = null;
                EventPublisher.Publish(new UserEmailAddressChanged(null, User.UserId, oldEmailAddress, Email));
            }

            return true;
        }

        public virtual bool Approve()
        {
            if (IsApproved)
                return false;

            IsApproved = true;
            return true;
        }

        public virtual void SetPassword(string password)
        {
            var newPassword = PasswordEncryptor.Encrypt(password, Salt);
            if (_password == newPassword) return;
            _password = newPassword;
            LastPasswordChangeDate = DateTime.UtcNow;
        }
    }
}