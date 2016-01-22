﻿namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;
    using Users.Model;

    public class MembershipApplication
    {
        private Guid _id;
        private Guid _organizationId;
        private UserId _userId;
        private DateTime _requestDate;        
        
        protected MembershipApplication()
        {
        }

        public MembershipApplication(Guid applicationId, Guid organizationId, UserId userId)
        {
            _id = applicationId;
            _organizationId = organizationId;
            _userId = userId;
            _requestDate = DateTime.UtcNow;
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Guid OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual UserId UserId
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
        public virtual MembershipApplicationId MembershipApplicationId { get {return new MembershipApplicationId(Id);} }


        public virtual Membership Approve(Guid membershipId)
        {
            ApprovalDate = DateTime.Now;

            return new Membership(membershipId, UserId, Id, ApprovalDate.Value, OrganizationId);
        }

        public virtual void Reject()
        {
            RejectDate = DateTime.Now;
        }
    }
}