namespace Phundus.Inventory.Projections
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Queries;
    using Stores.Model;

    public class StoresProjection : ProjectionBase<StoresRow>, IStoresQueries, IStoredEventsConsumer
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(StoreOpened e)
        {
            Insert(x =>
            {
                x.StoreId = e.StoreId;
                x.OwnerId = e.Owner.OwnerId.Id;
                x.OwnerType = e.Owner.Type.ToString().ToLowerInvariant();
            });
        }

        public void Process(AddressChanged e)
        {
            Update(e.StoreId, x =>
                x.Address = e.Address);
        }

        public void Process(OpeningHoursChanged e)
        {
            Update(e.StoreId, x =>
                x.OpeningHours = e.OpeningHours);
        }

        public void Process(CoordinateChanged e)
        {
            Update(e.StoreId, x =>
            {
                x.Latitude = e.Latitude;
                x.Longitude = e.Longitude;
            });
        }

        public StoresRow GetByOwnerId(OwnerId ownerId)
        {
            var result = FindByOwnerId(ownerId);
            if (result == null)
                throw new NotFoundException("Store with {0} not found.", ownerId);
            return result;
        }

        public StoresRow FindByOwnerId(OwnerId ownerId)
        {
            return Single(p => p.OwnerId == ownerId.Id);
        }
    }

    public class StoresRow
    {
        public virtual Guid StoreId { get; set; }
        public virtual Guid OwnerId { get; set; }
        public virtual string OwnerType { get; set; }
        public virtual string Address { get; set; }
        public virtual string OpeningHours { get; set; }
        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
    }
}