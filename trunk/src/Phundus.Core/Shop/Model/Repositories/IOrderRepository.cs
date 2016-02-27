namespace Phundus.Shop.Orders.Repositories
{
    using Common.Domain.Model;
    using Model;

    public interface IOrderRepository
    {
        Order GetById(OrderId orderId);
        void Add(Order aggregate);
        void Save(Order aggregate);
    }
}