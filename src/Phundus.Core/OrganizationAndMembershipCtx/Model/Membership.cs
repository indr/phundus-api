namespace Phundus.Core.OrganizationAndMembershipCtx.Model
{
    using System;

    public class Membership
    {
        private Guid _id;
        private int _memberId;
        private int _organizationId;
        private Guid _requestId;

        public Membership(Guid id, int organizationId, int memberId, Guid requestId)
        {
            _id = id;
            _organizationId = organizationId;
            _memberId = memberId;
            _requestId = requestId;
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int MemberId
        {
            get { return _memberId; }
            protected set { _memberId = value; }
        }

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual Guid RequestId
        {
            get { return _requestId; }
            protected set { _requestId = value; }
        }

        private DateTime _requestDate = DateTime.Now;
       
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
    }
}