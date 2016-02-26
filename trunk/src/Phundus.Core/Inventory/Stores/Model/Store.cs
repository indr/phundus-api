namespace Phundus.Inventory.Stores.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Inventory.Model;

    public class Store : EventSourcedAggregate
    {
        public Store(Manager manager, StoreId storeId, Owner owner)
        {
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
        public virtual Owner Owner { get; private set; }
        public virtual Coordinate Coordinate { get; private set; }
        public virtual string OpeningHours { get; private set; }
        public virtual string Address { get; private set; }
        public virtual string Name { get; private set; }

        protected void When(StoreOpened e)
        {
            StoreId = new StoreId(e.StoreId);
            Owner = e.Owner;
        }


        public void ChangeAddress(Manager manager, string address)
        {
            Apply(new AddressChanged(manager, StoreId, address));
        }

        protected void When(AddressChanged e)
        {
            Address = e.Address;
        }


        public void ChangeCoordinate(Manager manager, Coordinate coordinate)
        {
            Apply(new CoordinateChanged(manager, StoreId, coordinate));
        }

        protected void When(CoordinateChanged e)
        {
            Coordinate = new Coordinate(e.Latitude, e.Longitude);
        }


        public void ChangeOpeningHours(Manager manager, string openingHours)
        {
            Apply(new OpeningHoursChanged(manager, StoreId, openingHours));
        }

        protected void When(OpeningHoursChanged e)
        {
            OpeningHours = e.OpeningHours;
        }


        public void Rename(Manager manager, string name)
        {
            Apply(new StoreRenamed(manager, StoreId, name));
        }

        protected void When(StoreRenamed e)
        {
            Name = e.Name;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return StoreId;
        }

    }
}