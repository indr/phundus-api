namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System;

    public class Membership
    {
        private Guid _id;
        private int _memberId;
        private Guid _requestId;
        private int _version;
        private int _role;
        private DateTime _approvalDate;
        private Organization _organization;

        protected Membership()
        {
        }

        public Membership(Guid id, int memberId, Guid requestId, DateTime approvalDate)
        {
            _id = id;
            _memberId = memberId;
            _requestId = requestId;
            _role = 1;
            _approvalDate = approvalDate;
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual int MemberId
        {
            get { return _memberId; }
            protected set { _memberId = value; }
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

        public virtual int Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public virtual DateTime ApprovalDate
        {
            get { return _approvalDate; }
            protected set { _approvalDate = value; }
        }
    }
}