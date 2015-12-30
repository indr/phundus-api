namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using Common;
    using Core.Inventory.Stores.Model;
    using Core.Inventory.Stores.Repositories;

    public class NhStoreRepository : NhRepositoryBase<Store>, IStoreRepository
    {
        public Store GetById(StoreId storeId)
        {
            var result = FindById(storeId);
            if (result == null)
                throw new NotFoundException(String.Format("Store with id {0} not found.", storeId));
            return result;
        }
    }
}