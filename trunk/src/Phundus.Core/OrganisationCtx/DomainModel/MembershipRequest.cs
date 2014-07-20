namespace Phundus.Core.OrganisationCtx.DomainModel
{
    #region

    using System;
    using Ddd;

    #endregion

    public class Membership
    {
    }

    public class MembershipRequest
    {
        private Guid _id;
        private int _memberId;
        private int _organizationId;
        private DateTime _requestDate;

        protected MembershipRequest()
        {
        }

        public MembershipRequest(Guid requestId, int organizationId, int memberId)
        {
            _id = requestId;
            _organizationId = organizationId;
            _memberId = memberId;
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

        public virtual int MemberId
        {
            get { return _memberId; }
            protected set { _memberId = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual DateTime RequestDate
        {
            get { return _requestDate; }
            protected set { _requestDate = value; }
        }

        public virtual Membership Approve()
        {
            EventPublisher.Publish(new MembershipRequestApproved());

            throw new NotImplementedException();
        }

        public virtual void Reject()
        {
            EventPublisher.Publish(new MembershipRequestRejected());

            throw new NotImplementedException();
        }
    }
}