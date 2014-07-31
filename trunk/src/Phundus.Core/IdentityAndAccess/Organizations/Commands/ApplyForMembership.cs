namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Castle.Transactions;
    using Cqrs;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class ApplyForMembership
    {
        public int UserId { get; set; }
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
            var organization = OrganizationRepository.ById(command.OrganizationId);
            if (organization == null)
                throw new OrganizationNotFoundException();

            var user = UserRepository.ActiveById(command.UserId);
            if (user == null)
                throw new UserNotFoundException();

            var request = organization.RequestMembership(
                Requests.NextIdentity(),
                user);

            Requests.Add(request);
        }
    }
}