﻿namespace Phundus.Shop.Model
{
    using Common.Domain.Model;
    using Infrastructure;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetByUserGuid(UserId userId);
        Cart FindByUserGuid(UserId userId);
    }
}