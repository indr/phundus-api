namespace Phundus.Shop.Queries
{
    using System;
    using Common.Domain.Model;
    using Core.Cqrs;
    using Integration.Shop;

    public class CartReadModel : ReadModelBase, ICartQueries
    {
        public ICart FindByUserGuid(UserId userId, UserGuid userGuid)
        {
            throw new NotImplementedException();
        }
    }
}