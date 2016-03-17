namespace Phundus.Inventory.Model.Collaborators
{
    using Common.Domain.Model;

    public interface ICollaboratorService
    {        
        Manager Manager(UserId userId, OwnerId ownerId);
        bool IsMember(UserId userId, LessorId lessorId);
    }
}