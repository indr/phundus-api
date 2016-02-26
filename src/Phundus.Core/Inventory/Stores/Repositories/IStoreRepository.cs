namespace Phundus.Inventory.Stores.Repositories
{
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface IStoreRepository : IRepository<Store>
    {
        Store GetById(StoreId storeId);
    }

    public interface IStoreAggregateRepository
    {
        StoreAggregate GetById(StoreId storeId);
        void Save(StoreAggregate aggregate);
    }
}