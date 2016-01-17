namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;
    using Users.Model;
    using ApplicationId = Common.Domain.Model.ApplicationId;

    public class MembershipApplication
    {
        private Guid _id;
        private Guid _organizationId;
        private UserGuid _userGuid;
        private DateTime _requestDate;        
        
        protected MembershipApplication()
        {
        }

        public MembershipApplication(Guid applicationId, Guid organizationId, UserGuid userGuid)
        {
            _id = applicationId;
            _organizationId = organizationId;
            _userGuid = userGuid;
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

        public virtual UserGuid UserGuid
        {
            get { return _userGuid; }
            protected set { _userGuid = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual DateTime RequestDate
        {
            get { return _requestDate; }
            protected set { _requestDate = value; }
        }

        public virtual DateTime? ApprovalDate { get; protected set; }

        public virtual DateTime? RejectDate { get; protected set; }
        public virtual ApplicationId ApplicationId { get {return new ApplicationId(Id);} }


        public virtual Membership Approve(Guid membershipId)
        {
            ApprovalDate = DateTime.Now;

            return new Membership(membershipId, UserGuid, Id, ApprovalDate.Value, OrganizationId);
        }

        public virtual void Reject()
        {
            RejectDate = DateTime.Now;
        }
    }
}