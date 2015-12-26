namespace Phundus.Core.Inventory.Services
{
    using System;
    using Common;
    using IdentityAndAccess.Queries;
    using Stores.Model;

    public interface IOwnerService
    {
        Owner GetByUserId(int userId);
    }

    public class OwnerService : IOwnerService
    {
        private readonly IUserQueries _userQueries;

        public OwnerService(IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _userQueries = userQueries;
        }

        public Owner GetByUserId(int userId)
        {
            var user =_userQueries.ById(userId);
            if (user == null)
                throw new InvalidOperationException(String.Format("Owner could not be found."));

            return ToOwner(user);
        }

        private Owner ToOwner(UserDto user)
        {
            return new Owner(new OwnerId(user.Id.ToGuid()), user.FirstName + " " + user.LastName);
        }
    }
}