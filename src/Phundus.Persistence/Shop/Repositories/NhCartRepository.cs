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