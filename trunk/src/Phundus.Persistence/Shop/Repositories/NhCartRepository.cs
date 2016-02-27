﻿namespace Phundus.Persistence.Shop.Repositories
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Phundus.Shop.Model;

    public class NhCartRepository : NhRepositoryBase<Cart>, ICartRepository
    {
        public Cart GetByUserGuid(UserId userId)
        {
            var result = FindByUserGuid(userId);
            if (result == null)
                throw new NotFoundException("Cart with {0} not found.", userId);
            return result;
        }

        public Cart FindByUserGuid(UserId userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            var query = from c in Entities
                where c.UserId.Id == userId.Id
                select c;
            return query.SingleOrDefault();
        }
    }
}