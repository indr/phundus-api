namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Cqrs;
    using Queries;
    using Repositories;

    public class RejectMembershipApplication
    {
        public RejectMembershipApplication(InitiatorId initiatorId, MembershipApplicationId applicationId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            InitiatorId = initiatorId;
            ApplicationId = applicationId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public MembershipApplicationId ApplicationId { get; protected set; }
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

            MemberInRole.ActiveManager(application.OrganizationId, command.InitiatorId);

            organization.RejectMembershipRequest(command.InitiatorId, application);
        }
    }
}