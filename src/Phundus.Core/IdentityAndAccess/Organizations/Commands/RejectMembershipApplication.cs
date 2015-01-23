namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Common.Cqrs;
    using Cqrs;
    using Queries;
    using Repositories;

    public class RejectMembershipApplication : ICommand
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
            var application = MembershipRequestRepository.GetById(command.ApplicationId);

            var organization = OrganizationRepository.GetById(application.OrganizationId);

            MemberInRole.ActiveChief(application.OrganizationId, command.InitiatorId);

            organization.RejectMembershipRequest(application);
        }
    }
}