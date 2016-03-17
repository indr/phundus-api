namespace Phundus.IdentityAccess.Resources
{
    using System;
    using Application;
    using Castle.Transactions;
    using Common.Resources;
    using Model.Users;
    using Projections;

    public interface IUsersResource
    {
        UserData Get(Guid userId);
    }

    public class UsersResource : ResourceBase, IUsersResource
    {
        private readonly IUsersQueries _usersQueries;

        public UsersResource(IUsersQueries usersQueries)
        {
            _usersQueries = usersQueries;
        }

        [Transaction]
        public UserData Get(Guid userId)
        {
            return _usersQueries.FindById(userId);
        }
    }
}