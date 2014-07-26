namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System;

    public class MembershipRequest
    {
        private Guid _id;
        private int _userId;
        private int _organizationId;
        private DateTime _requestDate;

        protected MembershipRequest()
        {
        }

        public MembershipRequest(Guid requestId, int organizationId, int userId)
        {
            _id = requestId;
            _organizationId = organizationId;
            _userId = userId;
            _requestDate = DateTime.Now;
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual int UserId
        {
            get { return _userId; }
            protected set { _userId = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual DateTime RequestDate
        {
            get { return _requestDate; }
            protected set { _requestDate = value; }
        }

        public virtual DateTime? ApprovalDate { get; protected set; }

        public virtual DateTime? RejectDate { get; protected set; }

        public virtual Membership Approve(Guid membershipId)
        {
            ApprovalDate = DateTime.Now;
            

            return new Membership(membershipId, OrganizationId, UserId, Id);
        }

        public virtual void Reject()
        {
            RejectDate = DateTime.Now;
        }
    }
}