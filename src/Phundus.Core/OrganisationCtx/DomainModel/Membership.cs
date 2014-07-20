namespace Phundus.Core.OrganisationCtx.DomainModel
{
    using System;

    public class Membership
    {
        public virtual Guid Id { get; protected set; }

        public virtual int MemberId { get; protected set; }

        public virtual int OrganisationId { get; protected set; }
    }
}