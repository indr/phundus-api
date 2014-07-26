namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Infrastructure;
    using Repositories;

    public class ApproveMembershipRequest
    {
        public Guid RequestId { get; set; }
        public int MemberId { get; set; }
    }

    public class ApproveMembershipRequestHandler : IHandleCommand<ApproveMembershipRequest>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IMembershipRepository Memberships { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        [Transaction]
        public void Handle(ApproveMembershipRequest command)
        {
            var request = Requests.ById(command.RequestId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organization = OrganizationRepository.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");
            var membership = organization.ApproveMembershipRequest(request, Memberships.NextIdentity());

            Memberships.Add(membership);
        }
    }


}