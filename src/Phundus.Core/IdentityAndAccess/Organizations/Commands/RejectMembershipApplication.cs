namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Infrastructure;
    using Repositories;

    public class RejectMembershipApplication
    {
        public Guid ApplicationId { get; set; }

        public int AdministratorId { get; set; }
    }

    public class RejectMembershipRequestHandler : IHandleCommand<RejectMembershipApplication>
    {
        public IMembershipRequestRepository MembershipRequestRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        [Transaction]
        public void Handle(RejectMembershipApplication command)
        {
            var request = MembershipRequestRepository.ById(command.ApplicationId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organization = OrganizationRepository.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");
            organization.RejectMembershipRequest(request);
        }
    }
}