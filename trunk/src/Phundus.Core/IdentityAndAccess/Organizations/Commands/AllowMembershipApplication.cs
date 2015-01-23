namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Common.Cqrs;
    using Cqrs;
    using Queries;
    using Repositories;

    public class AllowMembershipApplication : ICommand
    {
        public Guid ApplicationId { get; set; }
        public int InitiatorId { get; set; }
    }

    public class AllowMembershipApplicationHandler : IHandleCommand<AllowMembershipApplication>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IMembershipRepository Memberships { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        [Transaction]
        public void Handle(AllowMembershipApplication command)
        {
            var application = Requests.GetById(command.ApplicationId);

            var organization = OrganizationRepository.GetById(application.OrganizationId);

            MemberInRole.ActiveChief(application.OrganizationId, command.InitiatorId);

            organization.ApproveMembershipRequest(application, Memberships.NextIdentity());
        }
    }
}