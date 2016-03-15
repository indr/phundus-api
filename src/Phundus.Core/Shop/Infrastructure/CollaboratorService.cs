namespace Phundus.Shop.Infrastructure
{
    using Common.Domain.Model;
    using IdentityAccess.Users.Services;
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

        public Manager Manager(UserId userId, LessorId lessorId)
        {
            var result = _userInRole.Manager(userId, new OrganizationId(lessorId.Id));
            return new Manager(result.UserId, result.EmailAddress, result.FullName);
        }
    }
}