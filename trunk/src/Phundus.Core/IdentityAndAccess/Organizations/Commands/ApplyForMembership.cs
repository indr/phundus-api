namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Castle.Transactions;
    using Cqrs;
    using Queries;
    using Repositories;
    using Users;
    using Users.Repositories;

    public class ApplyForMembership
    {
        public int ApplicantId { get; set; }
        public int OrganizationId { get; set; }
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

            //var user = UserRepository.ActiveById(command.ApplicantId);
            var user = UserRepository.FindById(command.ApplicantId);
            if (user == null)
                throw new UserNotFoundException(command.ApplicantId);

            var request = organization.RequestMembership(
                Requests.NextIdentity(),
                user);

            Requests.Add(request);
        }
    }
}