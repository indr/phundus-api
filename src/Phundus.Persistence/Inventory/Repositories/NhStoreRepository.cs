namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Phundus.Inventory.Stores.Model;
    using Phundus.Inventory.Stores.Repositories;

    public class NhStoreRepository : NhRepositoryBase<Store>, IStoreRepository
    {
        public Store GetById(StoreId storeId)
        {
            var result = FindById(storeId);
            if (result == null)
                throw new NotFoundException(String.Format("Store {0} not found.", storeId));
            return result;
        }

        public Store GetByOwnerAndId(OwnerId ownerId, StoreId storeId)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (storeId == null) throw new ArgumentNullException("storeId");

            var result = Entities.SingleOrDefault(p => p.Owner.OwnerId.Id == ownerId.Id && p.Id.Id == storeId.Id);
            if (result == null)
                throw new NotFoundException(String.Format("Store with {0} and {1} not found.", ownerId, storeId));
            return result;
        }
    }
}