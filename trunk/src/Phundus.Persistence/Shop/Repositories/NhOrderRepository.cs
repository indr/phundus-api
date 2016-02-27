namespace Phundus.Persistence.Shop.Repositories
{
    using Common;
    using Common.Domain.Model;
    using Inventory.Repositories;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;

    public class NhOrderRepository : EventSourcedRepositoryBase<Order>, IOrderRepository
    {
        public Order GetById(OrderId orderId)
        {
            return base.GetById(orderId);
        }

        public void Add(Order aggregate)
        {
            Save(aggregate);
        }

        public void Save(Order aggregate)
        {
            EventStore.AppendToStream(aggregate.OrderId, aggregate.MutatedVersion, aggregate.MutatingEvents);
        }
    }
}