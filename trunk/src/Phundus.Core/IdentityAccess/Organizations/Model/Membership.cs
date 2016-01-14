namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;

    public class Membership
    {
        private Guid _id;
        private int _userId;
        private Guid _requestId;
        private int _version;
        private Role _role;
        private DateTime _approvalDate;
        private Organization _organization;
        private bool _isLocked;
        private Guid _organizationGuid;

        protected Membership()
        {
        }

        public Membership(Guid id, int userId, Guid requestId, DateTime approvalDate, Guid organizationGuid)
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

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual int UserId
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