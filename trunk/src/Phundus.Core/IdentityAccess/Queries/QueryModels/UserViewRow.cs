namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;

    public class UserViewRow
    {
        public virtual int UserId { get; set; }
        public virtual Guid UserGuid { get; set; }

        public virtual string Username
        {
            get { return EmailAddress; }
        }

        public virtual int RoleId { get; set; }

        public virtual string EmailAddress { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string MobileNumber { get; set; }

        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }

        public virtual DateTime SignedUpAtUtc { get; set; }
        public virtual DateTime? LastLogInAtUtc { get; set; }
        public virtual DateTime? LastPasswordChangeAtUtc { get; set; }
        public virtual DateTime? LastLockOutAtUtc { get; set; }
    }
}