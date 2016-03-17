namespace Phundus.Inventory.Projections
{
    using System;
    using System.Collections.Generic;
    using Castle.Transactions;
    using Common;
    using Common.Querying;

    public interface IStoresQueryService
    {
        StoreDetailsData GetById(Guid storeId);
        StoreDetailsData GetByOwnerId(Guid ownerId);
        StoreDetailsData FindByOwnerId(Guid ownerId);
        IList<StoreDetailsData> Query(Guid? ownerId);
    }

    public class StoresQueryService : QueryServiceBase<StoreDetailsData>, IStoresQueryService
    {
        [Transaction]
        public StoreDetailsData GetById(Guid storeId)
        {
            return SingleOrThrow(p => p.StoreId == storeId, "Store {0} not found.", storeId);
        }

        [Transaction]
        public StoreDetailsData GetByOwnerId(Guid ownerId)
        {
            var result = FindByOwnerId(ownerId);
            if (result == null)
                throw new NotFoundException("Store with owner {0} not found.", ownerId);
            return result;
        }

        [Transaction]
        public StoreDetailsData FindByOwnerId(Guid ownerId)
        {
            return SingleOrDefault(p => p.OwnerId == ownerId);
        }

        public IList<StoreDetailsData> Query(Guid? ownerId)
        {
            if (!ownerId.HasValue)
                return new List<StoreDetailsData>();
            return QueryOver().Where(p => p.OwnerId == ownerId).List();
        }
    }
}