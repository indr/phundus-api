namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Cqrs;
    using Queries;
    using Repositories;

    public class ApproveMembershipApplication
    {
        public ApproveMembershipApplication(InitiatorGuid initiatorGuid, Guid applicationId)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            InitiatorGuid = initiatorGuid;
            ApplicationId = applicationId;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
        public Guid ApplicationId { get; protected set; }
    }

    public class AllowMembershipApplicationHandler : IHandleCommand<ApproveMembershipApplication>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IMembershipRepository Memberships { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        [Transaction]
        public void Handle(ApproveMembershipApplication command)
        {
            var application = Requests.GetById(command.ApplicationId);

            var organization = OrganizationRepository.GetById(application.OrganizationId);

            MemberInRole.ActiveChief(application.OrganizationId, command.InitiatorGuid);

            organization.ApproveMembershipRequest(command.InitiatorGuid, application, Memberships.NextIdentity());
        }
    }
}