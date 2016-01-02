﻿namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Core.Inventory.Stores.Model;
    using Core.Inventory.Stores.Repositories;

    public class NhStoreRepository : NhRepositoryBase<Store>, IStoreRepository
    {
        public Store GetById(StoreId storeId)
        {
            var result = FindById(storeId);
            if (result == null)
                throw new NotFoundException(String.Format("Store {0} not found.", storeId));
            return result;
        }
    }
}