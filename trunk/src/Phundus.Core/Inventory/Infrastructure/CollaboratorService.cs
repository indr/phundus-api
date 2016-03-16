namespace Phundus.Inventory.Infrastructure
{
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using Integration.IdentityAccess;
    using Model.Collaborators;
    using Manager = Model.Manager;

    public class CollaboratorService : ICollaboratorService
    {
        private readonly IUsersQueries _usersQueries;
        private readonly IUserInRole _userInRole;

        public CollaboratorService(IUsersQueries usersQueries, IUserInRole userInRole)
        {
            _usersQueries = usersQueries;
            _userInRole = userInRole;
        }

        public Initiator Initiator(InitiatorId initiatorId)
        {
            var result = _usersQueries.GetById(initiatorId.Id);
            return new Initiator(new InitiatorId(result.UserId), result.EmailAddress, result.FullName);
        }

        public Manager Manager(UserId userId, OwnerId ownerId)
        {
            var manager = _userInRole.Manager(userId, new OrganizationId(ownerId.Id));
            return new Manager(manager.UserId, manager.EmailAddress, manager.FullName);
        }
    }
}