namespace Phundus.Shop.Orders.Repositories
{
    using System;
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface ICartRepository : IRepository<Cart>
    {
        [Obsolete]
        Cart GetById(object id);

        Cart GetByUserGuid(UserGuid userGuid);
        Cart FindByUserGuid(UserGuid userGuid);
    }
}