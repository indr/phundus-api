namespace Phundus.Shop.Queries
{
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.Shop;

    public class CartReadModel : NHibernateReadModelBase<CartViewRow>, ICartQueries
    {
       public ICart FindByUserGuid(InitiatorId initiatorId, UserId userId)
        {
            if (initiatorId.Id != userId.Id)
                throw new AuthorizationException();

            return QueryOver().Where(p => p.UserGuid == userId.Id).SingleOrDefault();
        }
    }
}