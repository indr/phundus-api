namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Organizations;
    using Projections;

    public class ApproveMembershipApplication : ICommand
    {
        public ApproveMembershipApplication(InitiatorId initiatorId, MembershipApplicationId applicationId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            InitiatorId = initiatorId;
            ApplicationId = applicationId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public MembershipApplicationId ApplicationId { get; protected set; }
    }

    [Obsolete("Use constructor!")]
    public class ApproveMembershipApplicationHandler : IHandleCommand<ApproveMembershipApplication>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IMembershipRepository Memberships { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserInRoleService UserInRoleService { get; set; }

        [Transaction]
        public void Handle(ApproveMembershipApplication command)
        {
            var application = Requests.GetById(command.ApplicationId);

            var organization = OrganizationRepository.GetById(application.OrganizationId);

            var manager = UserInRoleService.Manager(command.InitiatorId, organization.Id);

            // TODO Pass manager
            organization.ApproveMembershipApplication(command.InitiatorId, application, Memberships.NextIdentity());
        }
    }
}