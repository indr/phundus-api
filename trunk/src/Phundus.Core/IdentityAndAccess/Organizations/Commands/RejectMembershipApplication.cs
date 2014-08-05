namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using System.Security;
    using Castle.Transactions;
    using Cqrs;
    using Queries;
    using Repositories;

    public class RejectMembershipApplication
    {
        public Guid ApplicationId { get; set; }
        public int InitiatorId { get; set; }
    }

    public class RejectMembershipRequestHandler : IHandleCommand<RejectMembershipApplication>
    {
        public IMembershipRequestRepository MembershipRequestRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        [Transaction]
        public void Handle(RejectMembershipApplication command)
        {
            var application = MembershipRequestRepository.ById(command.ApplicationId);
            if (application == null)
                throw new MembershipApplicationNotFoundException();

            var organization = OrganizationRepository.ById(application.OrganizationId);
            if (organization == null)
                throw new OrganizationNotFoundException();

            if (!MemberInRole.IsActiveChief(application.OrganizationId, command.InitiatorId))
                throw new SecurityException();

            organization.RejectMembershipRequest(application);
        }
    }
}