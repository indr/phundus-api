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
        public RejectMembershipApplication(InitiatorGuid initiatorGuid, Guid applicationId)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            InitiatorGuid = initiatorGuid;
            ApplicationId = applicationId;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
        public Guid ApplicationId { get; protected set; }
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

            MemberInRole.ActiveChief(application.OrganizationId, command.InitiatorGuid);

            organization.RejectMembershipRequest(command.InitiatorGuid, application);
        }
    }
}