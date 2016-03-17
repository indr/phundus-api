namespace Phundus.Shop.Infrastructure.Persistence.Repositories
{
    using Common.Domain.Model;
    using Common.Infrastructure.Persistence;
    using Shop.Model;

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