namespace Phundus.Inventory.Model.Collaborators
{
    using Common.Domain.Model;

    public interface IOwnerService
    {
        Owner GetById(OwnerId ownerId);
        Owner FindById(OwnerId ownerId);
    }
}