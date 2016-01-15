namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Users.Repositories;
    using Repositories;

    public class ApplyForMembership
    {
        public ApplyForMembership(InitiatorGuid initiatorGuid, Guid applicationId, UserGuid applicantId, Guid organizationId)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            InitiatorGuid = initiatorGuid;
            ApplicationId = applicationId;
            ApplicantId = applicantId;
            OrganizationId = organizationId;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
        public Guid ApplicationId { get; protected set; }
        public UserGuid ApplicantId { get; protected set; }
        public Guid OrganizationId { get; protected set; }
    }

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMembershipRequestRepository Requests { get; set; }

        [Transaction]
        public void Handle(ApplyForMembership command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var user = UserRepository.GetByGuid(command.ApplicantId);

            var request = organization.RequestMembership(command.InitiatorGuid, command.ApplicationId, user);

            Requests.Add(request);
        }
    }
}