namespace Phundus.Inventory.Projections
{
    using System;
    using Common.Notifications;
    using Common.Projecting;
    using Stores.Model;

    public class StoresProjection : ProjectionBase<StoreData>,
        IConsumes<StoreOpened>,
        IConsumes<StoreRenamed>,
        IConsumes<AddressChanged>,
        IConsumes<OpeningHoursChanged>,
        IConsumes<CoordinateChanged>
    {
        public void Handle(AddressChanged e)
        {
            Update(e.StoreId, x =>
                x.Address = e.Address);
        }

        public void Handle(CoordinateChanged e)
        {
            Update(e.StoreId, x =>
            {
                x.Latitude = e.Latitude;
                x.Longitude = e.Longitude;
            });
        }

        public void Handle(OpeningHoursChanged e)
        {
            Update(e.StoreId, x =>
                x.OpeningHours = e.OpeningHours);
        }

        public void Handle(StoreOpened e)
        {
            Insert(x =>
            {
                x.StoreId = e.StoreId;
                x.OwnerId = e.Owner.OwnerId.Id;
                x.OwnerType = e.Owner.Type.ToString().ToLowerInvariant();
            });
        }

        public void Handle(StoreRenamed e)
        {
            Update(e.StoreId, x =>
                x.Name = e.Name);
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