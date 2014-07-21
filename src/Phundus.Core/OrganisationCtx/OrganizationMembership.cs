namespace Phundus.Core.OrganisationCtx
{
    using System;
    using Ddd;
    using IdentityAndAccessCtx.DomainModel;

    public class OrganizationMembership : EntityBase
    {
        private DateTime _requestDate = DateTime.Now;
        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual int Role { get; set; }

        public virtual DateTime RequestDate
        {
            get { return _requestDate; }
            protected set { _requestDate = value; }
        }

        public virtual bool IsApproved { get; protected set; }

        public virtual DateTime? ApprovalDate { get; protected set; }

        public virtual bool IsLockedOut { get; protected set; }
        public virtual DateTime? LastLockoutDate { get; protected set; }

        public virtual void Lock()
        {
            // TODO: Audit
            // TODO: E-Mail an Benutzer senden
            IsLockedOut = true;
            LastLockoutDate = DateTime.Now;
        }

        public virtual void Unlock()
        {
            // TODO: Audit
            // TODO: E-Mail an Benutzer senden
            IsLockedOut = false;
        }

        public virtual void Approve()
        {
            // TODO: Audit
            // TODO: E-Mail an Benutzer senden
            IsApproved = true;
            ApprovalDate = DateTime.Now;
        }
    }
}