using System;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Entities
{
    public class Membership : EntityBase
    {
        private DateTime _createDate;
        private string _password;
        private string _salt;

        public Membership()
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

        protected virtual string Salt
        {
            get { return _salt; }
            set { _salt = value; }
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

        protected bool IsNotApproved
        {
            get { return !IsApproved; }
            set { IsApproved = !value; }
        }

        public string ValidationKey { get; protected set; }

        public void LockOut()
        {
            IsLockedOut = true;
            LastLockoutDate = DateTime.Now;
        }

        public void LogOn(string sessionKey, string password)
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

        public string GenerateValidationKey()
        {
            var key = KeyGenerator.CreateKey(24);
            ValidationKey = key;
            return key;
        }

        public bool ValidateValidationKey(string key)
        {
            if (key == ValidationKey)
                return IsApproved = true;
            return false;
        }

        public void Unlock()
        {
            IsLockedOut = false;
        }
    }
}