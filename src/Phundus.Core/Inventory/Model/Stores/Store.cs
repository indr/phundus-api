namespace Phundus.Inventory.Model.Stores
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Inventory.Stores.Model;

    public class Store : EventSourcedAggregateRoot
    {
        public Store(Manager manager, StoreId storeId, Owner owner)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeId, "StoreId must be provided.");
            AssertionConcern.AssertArgumentNotNull(owner, "Owner must be provided.");

            Apply(new StoreOpened(manager, storeId, owner));
        }

        protected Store()
        {
        }

        protected Store(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        public virtual StoreId StoreId { get; private set; }
        public virtual OwnerId OwnerId { get; private set; }
        public virtual Coordinate Coordinate { get; private set; }
        public virtual string OpeningHours { get; private set; }
        public virtual string Address { get; private set; }
        public virtual string Name { get; private set; }

        protected void When(StoreOpened e)
        {
            StoreId = new StoreId(e.StoreId);
            OwnerId = e.Owner.OwnerId;
        }


        public void ChangeAddress(Manager manager, string address)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(address, "Address must be provided.");

            Apply(new AddressChanged(manager, StoreId, address));
        }

        protected void When(AddressChanged e)
        {
            Address = e.Address;
        }


        public void ChangeCoordinate(Manager manager, Coordinate coordinate)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotNull(coordinate, "Coordinate must be provided.");

            Apply(new CoordinateChanged(manager, StoreId, coordinate));
        }

        protected void When(CoordinateChanged e)
        {
            Coordinate = new Coordinate(e.Latitude, e.Longitude);
        }


        public void ChangeOpeningHours(Manager manager, string openingHours)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotNull(openingHours, "Opening hours must be provided.");

            Apply(new OpeningHoursChanged(manager, StoreId, openingHours));
        }

        protected void When(OpeningHoursChanged e)
        {
            OpeningHours = e.OpeningHours;
        }


        public void Rename(Manager manager, string name)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            Apply(new StoreRenamed(manager, StoreId, name));
        }

        protected void When(StoreRenamed e)
        {
            Name = e.Name;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return OwnerId;
            yield return StoreId;
        }
    }
}