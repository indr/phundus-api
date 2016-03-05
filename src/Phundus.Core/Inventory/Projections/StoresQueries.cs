namespace Phundus.Inventory.Projections
{
    using System;
    using Common;
    using Common.Querying;

    public interface IStoresQueries
    {
        StoreData GetByOwnerId(Guid ownerId);
        StoreData FindByOwnerId(Guid ownerId);
    }

    public class StoresQueries : QueryBase<StoreData>, IStoresQueries
    {
        public StoreData GetByOwnerId(Guid ownerId)
        {
            var result = FindByOwnerId(ownerId);
            if (result == null)
                throw new NotFoundException("Store with owner {0} not found.", ownerId);
            return result;
        }

        public StoreData FindByOwnerId(Guid ownerId)
        {
            return SingleOrDefault(p => p.OwnerId == ownerId);
        }
    }
}