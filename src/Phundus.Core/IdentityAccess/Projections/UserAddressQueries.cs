namespace Phundus.IdentityAccess.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Querying;
    using Cqrs;
    using NHibernate;

    public interface IUserAddressQueries
    {
        UserAddressData FindById(InitiatorId initiatorId, Guid userId);
    }

    public class UserAddressQueries : QueryBase<UserAddressData>, IUserAddressQueries
    {
        public UserAddressData FindById(InitiatorId initiatorId, Guid userId)
        {
            var query = QueryOver();
            AuthFilter(query, initiatorId);
            query.Where(p => p.UserId == userId);

            return query.SingleOrDefault();
        }

        private static void AuthFilter(IQueryOver<UserAddressData, UserAddressData> query, InitiatorId initiatorId)
        {
            query.Where(p => p.UserId == initiatorId.Id);
        }
    }
}