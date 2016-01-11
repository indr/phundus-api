namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class ApplyForMembership
    {
        public int ApplicantId { get; set; }
        public Guid OrganizationId { get; set; }
    }

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        public IMembershipApplicationQueries ReadModel { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMembershipRequestRepository Requests { get; set; }

        [Transaction]
        public void Handle(ApplyForMembership command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var user = UserRepository.GetById(command.ApplicantId);

            var request = organization.RequestMembership(Guid.NewGuid(), user);

            Requests.Add(request);
        }
    }
}