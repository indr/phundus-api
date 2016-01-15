namespace Phundus.Persistence.Shop.Repositories
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Phundus.Shop.Orders;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;

    public class NhCartRepository : NhRepositoryBase<Cart>, ICartRepository
    {
        public Cart GetById(object id)
        {
            var result = FindById(id);
            if (result == null)
                throw new CartNotFoundException();
            return result;
        }

        public Cart FindByUserId(UserId userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            var query = from c in Entities
                where c.CustomerId == userId.Id
                select c;
            return query.SingleOrDefault();
        }

        public Cart GetByUserId(UserId userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            var result = FindByUserId(userId);
            if (result == null)
                throw new NotFoundException("Cart with {0} not found.", userId);

            return result;
        }

        public Cart GetByUserGuid(UserGuid userGuid)
        {
            var result = FindByUserGuid(userGuid);
            if (result == null)
                throw new NotFoundException("Cart with {0} not found.", userGuid);
            return result;
        }

        public Cart FindByUserGuid(UserGuid userGuid)
        {
            if (userGuid == null) throw new ArgumentNullException("userGuid");

            var query = from c in Entities
                        where c.UserGuid == userGuid.Id
                        select c;
            return query.SingleOrDefault();
        }
    }
}