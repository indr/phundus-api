namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System;

    public class Membership
    {
        private Guid _id;
        private int _memberId;
        private int _organizationId;
        private Guid _requestId;
        private int _version;

        protected Membership()
        {
        }

        public Membership(Guid id, int organizationId, int memberId, Guid requestId)
        {
            _id = id;
            _organizationId = organizationId;
            _memberId = memberId;
            _requestId = requestId;
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

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual Guid RequestId
        {
            get { return _requestId; }
            protected set { _requestId = value; }
        }

        public virtual Organization Organization { get; set; }

        public virtual int Role { get; set; }

        public virtual DateTime? ApprovalDate { get; protected set; }
    }
}