namespace Phundus.Core.Shop.Orders.Repositories
{
    using Infrastructure;
    using Model;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(int userId);
        Cart FindById(int id);
        Cart ById(object id);
        Cart GetById(object id);
    }
}