namespace Phundus.Inventory.Infrastructure
{
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Resources;
    using Model;
    using Model.Collaborators;

    public class CollaboratorService : ICollaboratorService
    {
        private readonly IMemberInRoleResource _memberInRoleResource;

        public CollaboratorService(IMemberInRoleResource memberInRoleResource)
        {
            _memberInRoleResource = memberInRoleResource;
        }

        public Manager Manager(UserId userId, OwnerId ownerId)
        {
            var manager = _memberInRoleResource.Manager(new OrganizationId(ownerId.Id), userId);
            if (manager == null)
                throw new NotFoundException("Manager {0} not found.", userId);
            return new Manager(new UserId(manager.UserId), manager.EmailAddress, manager.FullName);
        }

        public bool IsMember(UserId userId, LessorId lessorId)
        {
            var member = _memberInRoleResource.Member(new OrganizationId(lessorId.Id), userId);
            return member != null;
        }
    }
}