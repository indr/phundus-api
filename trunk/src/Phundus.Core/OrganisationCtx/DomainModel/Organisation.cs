namespace Phundus.Core.OrganisationCtx.DomainModel
{
    using System;
    using Ddd;
    using Iesi.Collections.Generic;

    public class Organisation
    {
        public virtual int Id { get; set; }
        public virtual int Version { get; set; }

        

        public virtual MembershipRequest RequestMembership(Guid requestId, Member member)
        {
            var request = new MembershipRequest(
                requestId,
                Id,
                member.Id);

            EventPublisher.Publish(new MembershipRequested());

            return request;
        }
    }
}