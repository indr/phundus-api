namespace Phundus.Core.IdentityAndAccessCtx.Model
{
    using System;
    using Ddd;
    using DomainModel;
    using Infrastructure;

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
        
        public virtual void LockOut()
        {
            IsLockedOut = true;
            LastLockoutDate = DateTime.Now;
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
            if (key == ValidationKey)
            {
                return IsApproved = true;
            }
            return false;
        }

        public virtual bool ValidateEmailKey(string key)
        {
            if (key == ValidationKey)
            {
                if (RequestedEmail != null)
                {
                    Email = RequestedEmail;
                    RequestedEmail = null;
                }
                return true;
            }
            return false;
        }

        public virtual void Unlock()
        {
            IsLockedOut = false;
        }
    }
}