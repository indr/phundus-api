namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Queries;
    using Repositories;

    public class AllowMembershipApplication
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
            var application = Requests.ById(command.ApplicationId);
            if (application == null)
                throw new MembershipApplicationNotFoundException();

            var organization = OrganizationRepository.ById(application.OrganizationId);
            if (organization == null)
                throw new OrganizationNotFoundException();

            MemberInRole.ActiveChief(application.OrganizationId, command.InitiatorId);

            organization.ApproveMembershipRequest(application, Memberships.NextIdentity());
        }
    }
}