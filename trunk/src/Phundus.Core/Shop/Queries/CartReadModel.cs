namespace Phundus.Shop.Queries
{
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.Shop;

    public class CartReadModel : NHibernateReadModelBase<CartViewRow>, ICartQueries
    {
        public ICart FindByUserGuid(UserId userId, UserGuid userGuid)
        {
            return QueryOver().Where(p => p.UserId == userId.Id && p.UserGuid == userGuid.Id).SingleOrDefault();
        }

        public ICart FindByUserGuid(InitiatorGuid initiatorGuid, UserGuid userGuid)
        {
            if (initiatorGuid.Id != userGuid.Id)
                throw new AuthorizationException();

            return QueryOver().Where(p => p.UserGuid == userGuid.Id).SingleOrDefault();
        }
    }
}