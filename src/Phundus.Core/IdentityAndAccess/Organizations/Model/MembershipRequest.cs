namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System;
    using Users.Model;

    public class MembershipRequest
    {
        private Guid _id;
        private int _userId;
        private int _organizationId;
        private DateTime _requestDate;
        private User _user;

        protected MembershipRequest()
        {
        }

        public MembershipRequest(Guid requestId, int organizationId, User user)
        {
            _id = requestId;
            _organizationId = organizationId;
            _user = user;
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

        public virtual User User
        {
            get { return _user; }
            protected set { _user = value; }
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