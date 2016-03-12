namespace Phundus.Persistence.Inventory.Repositories
{
    using Common.Domain.Model;
    using Phundus.Inventory.Model.Stores;
    using Phundus.Inventory.Stores.Model;

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