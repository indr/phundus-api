namespace Phundus.Shop.Model
{
    using Common.Domain.Model;

    public interface IOrderRepository
    {
        Order GetById(OrderId orderId);
        void Add(Order aggregate);
        void Save(Order aggregate);
    }
}