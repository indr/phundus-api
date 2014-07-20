﻿namespace Phundus.Core.OrganisationCtx.DomainModel
{
    #region

    using System;
    using Ddd;

    #endregion

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

        public virtual DateTime? ApprovalDate { get; protected set; }

        public virtual DateTime? RejectDate { get; protected set; }

        public virtual void Approve()
        {
            ApprovalDate = DateTime.Now;

            EventPublisher.Publish(new MembershipRequestApproved());
        }

        public virtual void Reject()
        {
            RejectDate = DateTime.Now;

            EventPublisher.Publish(new MembershipRequestRejected());
        }
    }
}