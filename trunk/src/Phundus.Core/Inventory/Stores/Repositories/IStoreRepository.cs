namespace Phundus.Inventory.Stores.Repositories
{
    using Common.Domain.Model;
    using Model;

    public interface IStoreRepository
    {
        Store GetById(StoreId storeId);
        void Add(Store aggregate);
        void Save(Store aggregate);
    }
}