﻿namespace Phundus.Core.Inventory.Stores.Repositories
{
    using Common;
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface IStoreRepository : IRepository<Store>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Store GetById(StoreId storeId);
    }
}