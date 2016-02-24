namespace Phundus.Inventory.Services
{
    using System;
    using Common.Domain.Model;
    using Model;

    public interface IUserInRole
    {
        Manager Manager(UserId userId, OwnerId ownerId);
    }

    public class UserInRole : IUserInRole
    {
        private readonly IdentityAccess.Users.Services.IUserInRole _userInRole;

        public UserInRole(Phundus.IdentityAccess.Users.Services.IUserInRole userInRole)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            _userInRole = userInRole;
        }

        public Manager Manager(UserId userId, OwnerId ownerId)
        {
            var manager = _userInRole.Manager(userId, new OrganizationId(ownerId.Id));

            return new Manager(manager.UserId, manager.EmailAddress, manager.FullName);
        }
    }
}