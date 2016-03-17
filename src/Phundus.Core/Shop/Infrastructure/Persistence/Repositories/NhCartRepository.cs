namespace Phundus.Shop.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Common.Infrastructure.Persistence;
    using Shop.Model;

    public class NhCartRepository : NhRepositoryBase<Cart>, ICartRepository
    {
        public Cart GetByUserGuid(UserId userId)
        {
            Cart result = FindByUserGuid(userId);
            if (result == null)
                throw new NotFoundException("Cart with {0} not found.", userId);
            return result;
        }

        public Cart FindByUserGuid(UserId userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            IQueryable<Cart> query = from c in Entities
                where c.UserId.Id == userId.Id
                select c;
            return query.SingleOrDefault();
        }
    }
}