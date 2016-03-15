namespace Phundus.Shop.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;    
    using IdentityAccess.Projections;
    using IdentityAccess.Resources;
    using Integration.IdentityAccess;
    using Model.Collaborators;
    using Manager = Model.Manager;

    public class CollaboratorService : ICollaboratorService
    {
        private readonly IUserInRole _userInRole;
        private readonly IMemberQueries _memberQueries;
        private readonly IUsersQueries _usersQueries;

        public CollaboratorService(IUsersQueries usersQueries, IUserInRole userInRole, IMemberQueries memberQueries)
        {
            _usersQueries = usersQueries;
            _userInRole = userInRole;
            _memberQueries = memberQueries;
        }

        public Initiator Initiator(InitiatorId initiatorId)
        {
            var result = _usersQueries.GetById(initiatorId.Id);
            return new Initiator(new InitiatorId(result.UserId), result.EmailAddress, result.FullName);
        }

        public Manager Manager(LessorId lessorId, UserId userId)
        {
            var result = _userInRole.Manager(userId, new OrganizationId(lessorId.Id));
            return new Manager(result.UserId, result.EmailAddress, result.FullName);
        }

        public ICollection<Manager> Managers(LessorId lessorId, bool emailSubscription)
        {
            var managers = _memberQueries.Managers(lessorId.Id, emailSubscription);

            return managers.Select(s => new Manager(new UserId(s.Guid), s.EmailAddress, s.FullName)).ToList();
        }
    }
}