namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Infrastructure;
    using Model;
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
            Organization organization = OrganizationRepository.ById(command.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");

            var user = UserRepository.FindById(command.UserId);
            Guard.Against<EntityNotFoundException>(user == null, "User not found");

            var request = organization.RequestMembership(
                Requests.NextIdentity(),
                user);

            Requests.Add(request);
        }
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}