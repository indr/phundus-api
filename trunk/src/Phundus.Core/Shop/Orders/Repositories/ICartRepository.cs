namespace Phundus.Core.Shop.Orders.Repositories
{
    using Infrastructure;
    using Model;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetById(object id);
        
        Cart FindByCustomer(int userId);
    }
}