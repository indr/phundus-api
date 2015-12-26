namespace Phundus.Core.Inventory.Stores.Repositories
{
    using Infrastructure;
    using Model;

    public interface IStoreRepository : IRepository<Store>
    {
        Store GetById(StoreId storeId);
    }
}