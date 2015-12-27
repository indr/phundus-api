namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using Core.Inventory.Stores.Model;
    using Core.Inventory.Stores.Repositories;
    using Infrastructure;

    public class NhStoreRepository : NhRepositoryBase<Store>, IStoreRepository
    {
        public Store GetById(StoreId storeId)
        {
            var result = FindById(storeId);
            if (result == null)
                throw new NotFoundException("Store not found.");
            return result;
        }
    }
}