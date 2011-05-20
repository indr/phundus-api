﻿using System;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Membership : BaseEntity
    {
        public Membership() : base()
        {
            _createDate = DateTime.Now;
        }

        public virtual User User { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }
        private DateTime _createDate;
        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }
        public virtual DateTime? LastLogOnDate { get; set; }
        public virtual DateTime? LastPasswordChangeDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual string Comment { get; set; }
    }
}
