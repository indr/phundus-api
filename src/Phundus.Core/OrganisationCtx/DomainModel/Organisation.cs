namespace Phundus.Core.OrganisationCtx.DomainModel
{
    using System;
    using Ddd;

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

        public virtual Membership ApproveMembershipRequest(MembershipRequest request, Guid membershipId)
        {
            var membership = request.Approve(membershipId);

            EventPublisher.Publish(new MembershipRequestApproved());

            return membership;
        }

        public virtual void RejectMembershipRequest(MembershipRequest request)
        {
            request.Reject();

            EventPublisher.Publish(new MembershipRequestRejected());
        }
    }
}