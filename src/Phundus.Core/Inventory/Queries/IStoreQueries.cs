namespace Phundus.Core.Inventory.Queries
{
    using System;

    public interface IStoreQueries
    {
        StoreDto FindByUserId(Guid userId);
    }
}