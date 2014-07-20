namespace Phundus.Core.OrganisationCtx.Commands
{
    using System;
    using Cqrs;
    using DomainModel;
    using Phundus.Infrastructure;
    using Repositories;
    using Services;

    public class ApplyForMembership
    {
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        public IOrganisationRepository Organizations { get; set; }

        public IMemberService Members { get; set; }

        public IMembershipRequestRepository Requests { get; set; }

        public void Handle(ApplyForMembership command)
        {
            var organization = Organizations.ById(command.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");

            var member = Members.MemberFrom(command.MemberId);
            Guard.Against<EntityNotFoundException>(member == null, "Member not found");

            var request = organization.CreateMembershipRequest(Requests.NextIdentity(), member);
            Requests.Add(request);
        }
    }

    public class EntityNotFoundException : Exception
    {
    }
}