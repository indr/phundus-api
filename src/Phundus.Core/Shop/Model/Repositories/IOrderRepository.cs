namespace Phundus.Shop.Orders.Repositories
{
    using Infrastructure;
    using Model;

    public interface IOrderRepository : IRepository<Order>
    {
        Order GetById(int orderId);
    }
}