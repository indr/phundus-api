namespace Phundus.Inventory.Projections
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Stores.Model;

    public interface IStoresQueries
    {
        StoreData GetByOwnerId(Guid ownerId);
        StoreData FindByOwnerId(Guid ownerId);
    }

    public class StoresProjection : ProjectionBase<StoreData>, IStoresQueries, IStoredEventsConsumer
    {
        public override void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

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

        private void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        private void Process(StoreOpened e)
        {
            Insert(x =>
            {
                x.StoreId = e.StoreId;
                x.OwnerId = e.Owner.OwnerId.Id;
                x.OwnerType = e.Owner.Type.ToString().ToLowerInvariant();
            });
        }

        private void Process(StoreRenamed e)
        {
            Update(e.StoreId, x =>
                x.Name = e.Name);
        }

        private void Process(AddressChanged e)
        {
            Update(e.StoreId, x =>
                x.Address = e.Address);
        }

        private void Process(OpeningHoursChanged e)
        {
            Update(e.StoreId, x =>
                x.OpeningHours = e.OpeningHours);
        }

        private void Process(CoordinateChanged e)
        {
            Update(e.StoreId, x =>
            {
                x.Latitude = e.Latitude;
                x.Longitude = e.Longitude;
            });
        }
    }

    public class StoreData
    {
        public virtual Guid StoreId { get; set; }
        public virtual Guid OwnerId { get; set; }
        public virtual string OwnerType { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string OpeningHours { get; set; }
        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
    }
}