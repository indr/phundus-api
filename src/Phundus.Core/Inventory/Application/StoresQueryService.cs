namespace Phundus.Inventory.Application
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

    public class StoreDetailsData
    {
        public virtual Guid OwnerId { get; set; }
        public virtual string OwnerType { get; set; }
        public virtual Guid StoreId { get; set; }
        public virtual string Name { get; set; }

        public virtual string Line1 { get; set; }
        public virtual string Line2 { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalAddress { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }

        public virtual string OpeningHours { get; set; }
        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
    }
}