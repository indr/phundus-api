using System;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Membership : BaseEntity
    {
        private DateTime _createDate;

        public Membership()
        {
            _createDate = DateTime.Now;
        }

        public virtual User User { get; set; }
        public virtual string Password { get; set; }
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

        public void LogOn(string password)
        {
            Guard.Against<ArgumentNullException>(password == null, "pasword");
            Guard.Against<UserLookedOutException>(IsLockedOut, "");
            Guard.Against<InvalidPasswordException>(Password != password, "");

            // TODO,Inder: Generate Session-Key without Captain Obvious
            SessionKey = "Session.Key";
            LastLogOnDate = DateTime.Now;
        }
    }
}