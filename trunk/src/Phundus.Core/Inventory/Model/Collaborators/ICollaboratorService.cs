namespace Phundus.Inventory.Model.Collaborators
{
    using Common.Domain.Model;

    public interface ICollaboratorService
    {
        Initiator Initiator(InitiatorId initiatorId);
        Manager Manager(UserId userId, OwnerId ownerId);
    }
}