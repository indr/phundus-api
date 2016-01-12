namespace Phundus.Persistence.Shop.Repositories
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;

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
    }
}