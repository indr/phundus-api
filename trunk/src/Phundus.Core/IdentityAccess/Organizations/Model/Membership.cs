namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;

    public class Membership
    {
        private DateTime _approvalDate;
        private Guid _id;
        private bool _isLocked;
        private Organization _organization;
        private Guid _organizationGuid;
        private Guid _requestId;
        private Role _role;
        private UserId _userId;
        private int _version;

        protected Membership()
        {
        }

        public Membership(Guid id, UserId userId, Guid requestId, DateTime approvalDate, Guid organizationGuid)
        {
            _id = id;
            _userId = userId;
            _requestId = requestId;
            _role = Role.Member;
            _approvalDate = approvalDate;
            _isLocked = false;
            _organizationGuid = organizationGuid;
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual Guid OrganizationGuid
        {
            get { return _organizationGuid; }
            protected set { _organizationGuid = value; }
        }

        public virtual OrganizationId OrganizationId
        {
            get { return new OrganizationId(OrganizationGuid); }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual UserId UserId
        {
            get { return _userId; }
            protected set { _userId = value; }
        }

        public virtual Guid RequestId
        {
            get { return _requestId; }
            protected set { _requestId = value; }
        }

        public virtual Organization Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        public virtual Role Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public virtual DateTime ApprovalDate
        {
            get { return _approvalDate; }
            protected set { _approvalDate = value; }
        }

        public virtual bool IsLocked
        {
            get { return _isLocked; }
            protected set { _isLocked = value; }
        }


        public virtual void ChangeRole(Role role)
        {
            Role = role;
        }

        public virtual void Lock()
        {
            IsLocked = true;
        }

        public virtual void Unlock()
        {
            IsLocked = false;
        }
    }
}