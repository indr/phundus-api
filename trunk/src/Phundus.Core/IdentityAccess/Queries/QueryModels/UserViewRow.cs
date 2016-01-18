namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;
    using Common.Domain.Model;
    using Integration.IdentityAccess;

    public class MembershipApplicationViewRow : IMembershipApplication
    {
        public virtual Guid ApplicationId { get; protected set; }
        public virtual Guid OrganizationId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual string CustomMemberNumber { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string EmailAddress { get; protected set; }
        public virtual DateTime RequestedAtUtc { get; protected set; }
        public virtual DateTime? ApprovedAtUtc { get; protected set; }
        public virtual DateTime? RejectedAtUtc { get; protected set; }
    }

    public class UserViewRow : IUser
    {
        public virtual Guid UserId { get; set; }

        public virtual string Username
        {
            get { return EmailAddress; }
        }

        public virtual int RoleId { get; set; }

        public virtual string RoleName
        {
            get
            {
                if (RoleId == (int) UserRole.Admin)
                    return "Admin";
                if (RoleId == (int) UserRole.User)
                    return "User";
                return null;
            }
        }

        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Für E-Mail-Template!
        /// </summary>
        public virtual string Email { get { return EmailAddress; } }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual int? JsNummer { get; set; }

        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }

        public virtual DateTime SignedUpAtUtc { get; set; }
        public virtual DateTime? LastLogInAtUtc { get; set; }
        public virtual DateTime? LastPasswordChangeAtUtc { get; set; }
        public virtual DateTime? LastLockOutAtUtc { get; set; }

        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}