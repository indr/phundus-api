namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Infrastructure;
    using Orders.Model;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetById(object id);
        
        Cart FindByCustomer(int userId);
    }
}