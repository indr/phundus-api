namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Infrastructure;
    using Repositories;

    public class AllowMembershipApplication
    {
        public Guid ApplicationId { get; set; }

        public int AdministratorId { get; set; }
    }

    public class AllowMembershipApplicationHandler : IHandleCommand<AllowMembershipApplication>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IMembershipRepository Memberships { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        [Transaction]
        public void Handle(AllowMembershipApplication command)
        {
            var request = Requests.ById(command.ApplicationId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organization = OrganizationRepository.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");
            organization.ApproveMembershipRequest(request, Memberships.NextIdentity());
        }
    }


}