namespace Phundus.Inventory.Services
{
    using Common.Domain.Model;
    using Model;

    public interface IUserInRole
    {
        Manager Manager(UserId userId, OwnerId ownerId);
    }

    public class UserInRole : IUserInRole
    {
        public Manager Manager(UserId userId, OwnerId ownerId)
        {
            throw new System.NotImplementedException();
        }
    }
}