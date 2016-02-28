namespace Phundus.IdentityAccess.Projections
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using NHibernate;

    public interface IUserAddressQueries
    {
        UserAddress GetById(InitiatorId initiatorId, Guid userId);
    }

    public class UserAddressQueries : QueryBase<UserAddress>, IUserAddressQueries
    {
        public UserAddress GetById(InitiatorId initiatorId, Guid userId)
        {
            var query = QueryOver();
            AuthFilter(query, initiatorId);
            query.Where(p => p.UserId == userId);

            var result = query.SingleOrDefault();
            if (result == null)
                throw new NotFoundException("User address {0} not found.", userId);
            return result;
        }

        private static void AuthFilter(IQueryOver<UserAddress, UserAddress> query, InitiatorId initiatorId)
        {
            query.Where(p => p.UserId == initiatorId.Id);
        }
    }
}