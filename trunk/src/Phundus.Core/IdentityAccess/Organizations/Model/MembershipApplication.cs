namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;

    public class MembershipApplication
    {
        private MembershipApplicationId _applicationId;
        private OrganizationId _organizationId;
        private DateTime _requestedAtUtc;
        private UserId _userId;

        public MembershipApplication(MembershipApplicationId applicationId, OrganizationId organizationId, UserId userId)
        {
            _applicationId = applicationId;
            _organizationId = organizationId;
            _userId = userId;
            _requestedAtUtc = DateTime.UtcNow;
        }

        protected MembershipApplication()
        {
        }

        public virtual MembershipApplicationId MembershipApplicationId
        {
            get { return _applicationId; }
            protected set { _applicationId = value; }
        }

        public virtual OrganizationId OrganizationId
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

        public virtual DateTime RequestedAtUtc
        {
            get { return _requestedAtUtc; }
            protected set { _requestedAtUtc = value; }
        }

        public virtual DateTime? ApprovedAtUtc { get; protected set; }

        public virtual DateTime? RejectedAtUtc { get; protected set; }

        


        public virtual Membership Approve(Guid membershipId)
        {
            ApprovedAtUtc = DateTime.UtcNow;

            return new Membership(membershipId, UserId, ApprovedAtUtc.Value, OrganizationId);
        }

        public virtual void Reject()
        {
            RejectedAtUtc = DateTime.UtcNow;
        }
    }
}