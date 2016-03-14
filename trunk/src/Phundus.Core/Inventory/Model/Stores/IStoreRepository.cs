namespace Phundus.Inventory.Model.Stores
{
    using Common.Domain.Model;

    public interface IStoreRepository
    {
        Store GetById(StoreId storeId);
        void Add(Store aggregate);
        void Save(Store aggregate);
    }
}