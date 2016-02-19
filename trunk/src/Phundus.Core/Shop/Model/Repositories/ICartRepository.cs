namespace Phundus.Shop.Orders.Repositories
{
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetByUserGuid(UserId userId);
        Cart FindByUserGuid(UserId userId);
    }
}