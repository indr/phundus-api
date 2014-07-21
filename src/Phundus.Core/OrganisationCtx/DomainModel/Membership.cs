namespace Phundus.Core.OrganisationCtx.DomainModel
{
    using System;

    public class Membership
    {
        private Guid _id;
        private int _memberId;
        private int _organizationId;
        private Guid _requestId;

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
    }
}