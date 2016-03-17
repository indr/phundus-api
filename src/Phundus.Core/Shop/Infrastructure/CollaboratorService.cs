namespace Phundus.Shop.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using IdentityAccess.Model;
    using IdentityAccess.Resources;
    using Model;
    using Model.Collaborators;

    public class CollaboratorService : ICollaboratorService
    {
        private readonly IMemberQueryService _memberQueryService;
        private readonly IUserInRoleService _userInRoleService;
        private readonly IUsersResource _usersQueries;

        public CollaboratorService(IUsersResource usersQueries, IUserInRoleService userInRoleService,
            IMemberQueryService memberQueryService)
        {
            _usersQueries = usersQueries;
            _userInRoleService = userInRoleService;
            _memberQueryService = memberQueryService;
        }

        public Initiator Initiator(InitiatorId initiatorId)
        {
            var result = _usersQueries.Get(initiatorId.Id);
            if (result == null)
                throw new NotFoundException("Initiator {0} not found.", initiatorId);
            return new Initiator(new InitiatorId(result.UserId), result.EmailAddress, result.FullName);
        }

        public Manager Manager(LessorId lessorId, UserId userId)
        {
            var result = _userInRoleService.Manager(userId, new OrganizationId(lessorId.Id));
            return new Manager(result.UserId, result.EmailAddress, result.FullName);
        }

        public ICollection<Manager> Managers(LessorId lessorId, bool emailSubscription)
        {
            var managers = _memberQueryService.Managers(lessorId.Id, emailSubscription);

            return managers.Select(s => new Manager(new UserId(s.Guid), s.EmailAddress, s.FullName)).ToList();
        }
    }
}