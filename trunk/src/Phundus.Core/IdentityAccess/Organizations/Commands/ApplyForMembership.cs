namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using IdentityAccess.Users.Repositories;
    using Repositories;

    public class ApplyForMembership
    {
        public ApplyForMembership(Guid applicationId, int applicantId, Guid organizationId)
        {
            ApplicationId = applicationId;
            ApplicantId = applicantId;
            OrganizationId = organizationId;
        }

        public Guid ApplicationId { get; protected set; }
        public int ApplicantId { get; protected set; }
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

            var user = UserRepository.GetById(command.ApplicantId);

            var request = organization.RequestMembership(command.ApplicationId, user);

            Requests.Add(request);
        }
    }
}