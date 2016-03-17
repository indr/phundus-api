namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using Common.Domain.Model;
    using IdentityAccess.Model.Users;

    public class Membership
    {
        private DateTime _approvedAtUtc;
        private Guid _id;
        private bool _isLocked;
        private Organization _organization;
        private Guid _organizationGuid;
        private bool _recievesEmailNotifications;
        private MemberRole _memberRole;
        private UserId _userId;
        private int _version;

        public Membership(Guid id, UserId userId, DateTime approvedAtUtc, OrganizationId organizationId)
        {
            _id = id;
            _userId = userId;
            _memberRole = MemberRole.Member;
            _approvedAtUtc = approvedAtUtc;
            _isLocked = false;
            _organizationGuid = organizationId.Id;
        }

        protected Membership()
        {
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual OrganizationId OrganizationId
        {
            get { return Organization.Id; }
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

        public virtual Organization Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        public virtual MemberRole MemberRole
        {
            get { return _memberRole; }
            protected set { _memberRole = value; }
        }

        public virtual DateTime ApprovedAtUtc
        {
            get { return _approvedAtUtc; }
            protected set { _approvedAtUtc = value; }
        }

        public virtual bool IsLocked
        {
            get { return _isLocked; }
            protected set { _isLocked = value; }
        }

        public virtual bool RecievesEmailNotifications
        {
            get { return _recievesEmailNotifications; }
            protected set { _recievesEmailNotifications = value; }
        }

        public virtual void ChangeRole(MemberRole memberRole)
        {
            MemberRole = memberRole;
            RecievesEmailNotifications = memberRole == MemberRole.Manager;
        }

        public virtual void Lock(Manager manager)
        {
            if (Equals(UserId, manager.UserId))
                throw new InvalidOperationException("You can not lock your own membership.");

            IsLocked = true;
        }

        public virtual void Unlock(Manager manager)
        {
            if (Equals(UserId, manager.UserId))
                throw new InvalidOperationException("You can not unlock your own membership.");

            IsLocked = false;
        }

        public virtual void SetRecievesEmailNotification(bool value)
        {
            RecievesEmailNotifications = value;
        }
    }
}