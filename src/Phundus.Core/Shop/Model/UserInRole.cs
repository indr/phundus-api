namespace Phundus.Shop.Model
{
    using System;
    using Common.Domain.Model;

    public interface IUserInRole
    {
        Manager Manager(UserId userId, LessorId lessorId);
    }

    public class UserInRole : IUserInRole
    {
        private readonly IdentityAccess.Users.Services.IUserInRole _userInRole;

        public UserInRole(IdentityAccess.Users.Services.IUserInRole userInRole)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            _userInRole = userInRole;
        }

        public Manager Manager(UserId userId, LessorId lessorId)
        {
            var manager = _userInRole.Manager(userId, new OrganizationId(lessorId.Id));

            return new Manager(manager.UserId, manager.EmailAddress, manager.FullName);
        }
    }
}