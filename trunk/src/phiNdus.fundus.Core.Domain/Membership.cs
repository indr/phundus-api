using System;

namespace phiNdus.fundus.Core.Domain
{
    public class Membership : BaseEntity
    {
        public Membership() : this(0)
        {
            
        }

        public Membership(int id) : base(id)
        {
            CreateDate = DateTime.Now;
        }

        public virtual User User { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordQuestion { get; set; }
        public virtual string PasswordAnswer { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }
        public virtual DateTime? LastPasswordChangeDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual string Comment { get; set; }
    }
}
