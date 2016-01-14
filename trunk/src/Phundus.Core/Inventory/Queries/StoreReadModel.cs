namespace Phundus.Inventory.Queries
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using NHibernate.Linq;
    using Stores.Model;
    using Stores.Repositories;

    public class StoreReadModel : ReadModelBase, IStoreQueries
    {
        public IStoreRepository StoreRepository { get; set; }

        public StoreDto GetByOwnerId(OwnerId ownerId)
        {
            var result = FindByOwnerId(ownerId);
            if (result == null)
                throw new NotFoundException(String.Format("No store with owner id {0} found.", ownerId));
            return result;
        }

        public StoreDto FindByOwnerId(OwnerId ownerId)
        {
            var store = Session.Query<Store>().FirstOrDefault(p => p.Owner.OwnerId.Id == ownerId.Id);
            if (store == null)
                return null;

            return new StoreDto
            {
                Latitude = store.Coordinate != null ? store.Coordinate.Latitude : (decimal?) null,
                Longitude = store.Coordinate != null ? store.Coordinate.Longitude : (decimal?) null,
                StoreId = store.Id,
                Address = store.Address,
                OpeningHours = store.OpeningHours
            };
        }
    }
}