namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Cqrs;
    using NHibernate.Linq;
    using Owners;
    using Stores.Model;
    using Stores.Repositories;

    public class StoreReadModel : ReadModelBase, IStoreQueries
    {
        public IStoreRepository StoreRepository { get; set; }

        public StoreDto FindByUserId(Guid userId)
        {
            var store = Session.Query<Store>().Where(p => p.Owner.OwnerId == new OwnerId(userId)).FirstOrDefault();
            if (store == null)
                return null;

            return new StoreDto()
            {   
                Latitude = store.Coordinate != null ? store.Coordinate.Latitude : (decimal?)null,
                Longitude = store.Coordinate != null ? store.Coordinate.Longitude : (decimal?)null,
                StoreId = store.Id.Value,
                Address = store.Address,
                OpeningHours = store.OpeningHours
            };
        }
    }
}