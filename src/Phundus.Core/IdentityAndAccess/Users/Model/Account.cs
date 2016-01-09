namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using System;
    using Ddd;
    using Exceptions;
    using Infrastructure;
    using Services;

    public class Account : EntityBase
    {
        private DateTime _createDate;
        private string _password;
        private string _salt;

        public Account()
        {
            _createDate = DateTime.Now;
            _salt = KeyGenerator.CreateKey(5);
        }

        public virtual User User { get; set; }

        public virtual string Password
        {
            get { return _password; }
            set
            {
                var newPassword = PasswordEncryptor.Encrypt(value, Salt);
                if (_password == newPassword) return;
                _password = newPassword;
                LastPasswordChangeDate = DateTime.Now;
            }
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

        public virtual void Lock(User initiator)
        {
            if (IsLockedOut)
                return;

            IsLockedOut = true;
            LastLockoutDate = DateTime.UtcNow;

            EventPublisher.Publish(new UserLocked(initiator, User, LastLockoutDate.Value));
        }

        public virtual void Unlock(User initiator)
        {
            if (!IsLockedOut)
                return;

            IsLockedOut = false;

            EventPublisher.Publish(new UserUnlocked(initiator, User, LastLockoutDate.GetValueOrDefault()));
        }

        public virtual void LogOn(string sessionKey, string password)
        {
            Guard.Against<ArgumentNullException>(sessionKey == null, "sessionKey");
            Guard.Against<ArgumentException>(String.IsNullOrEmpty(sessionKey), "sessionKey must not be empty");
            Guard.Against<ArgumentNullException>(password == null, "password");
            Guard.Against<UserNotApprovedException>(IsNotApproved, "");
            Guard.Against<UserLockedOutException>(IsLockedOut, "");

            Guard.Against<InvalidPasswordException>(
                Password != PasswordEncryptor.Encrypt(password, Salt), "");

            SessionKey = sessionKey;
            LastLogOnDate = DateTime.Now;

            EventPublisher.Publish(new UserLoggedIn(User.Id));
        }

        public virtual void ChangePassword(string oldPassword, string newPassword)
        {
            Guard.Against<ArgumentNullException>(oldPassword == null, "oldPassword");
            Guard.Against<ArgumentNullException>(newPassword == null, "newPassword");

            Guard.Against<InvalidPasswordException>(
                Password != PasswordEncryptor.Encrypt(oldPassword, Salt), "Das alte Passwort ist falsch.");

            Password = newPassword;
        }

        public virtual string ResetPassword()
        {
            var result = PasswordGenerator.CreatePassword();
            Password = result;
            return result;
        }

        public virtual string GenerateValidationKey()
        {
            var key = KeyGenerator.CreateKey(24);
            ValidationKey = key;
            return key;
        }

        public virtual bool ValidateValidationKey(string key)
        {
            return ValidateKey(key);
        }

        public virtual bool ValidateEmailKey(string key)
        {
            return ValidateKey(key);
        }

        private bool ValidateKey(string key)
        {
            if (key != ValidationKey)
                return false;

            IsApproved = true;
            if (RequestedEmail != null)
            {
                Email = RequestedEmail;
                RequestedEmail = null;
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
    }
}