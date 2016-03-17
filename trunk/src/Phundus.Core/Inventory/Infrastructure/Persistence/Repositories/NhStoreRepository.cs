namespace Phundus.Inventory.Infrastructure.Persistence.Repositories
{
    using Common.Domain.Model;
    using Common.Infrastructure.Persistence;
    using Model.Stores;

    public class EventSourcedStoreRepository : EventSourcedRepositoryBase<Store>, IStoreRepository
    {
        public Store GetById(StoreId storeId)
        {
            return base.GetById(storeId);
        }

        public void Add(Store aggregate)
        {
            Save(aggregate);
        }

        public void Save(Store aggregate)
        {
            EventStore.AppendToStream(aggregate.StoreId, aggregate.MutatedVersion, aggregate.MutatingEvents);
        }
    }
}