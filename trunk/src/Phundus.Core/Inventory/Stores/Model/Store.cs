namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Ddd;
    using Inventory.Model;

    public class StoreAggregate : EventSourcedRootEntity
    {
        public StoreAggregate(Manager manager, StoreId storeId, Owner owner)
        {
            Apply(new StoreOpened(manager, storeId, owner));
        }

        protected StoreAggregate()
        {
        }

        public virtual StoreId StoreId { get; private set; }
        public virtual Owner Owner { get; private set; }
        public virtual Coordinate Coordinate { get; private set; }
        public virtual string OpeningHours { get; private set; }
        public virtual string Address { get; private set; }

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

        protected void When(CoordinateChanged e)
        {
            Coordinate = new Coordinate(e.Latitude, e.Longitude);
        }

        protected void When(OpeningHoursChanged e)
        {
            OpeningHours = e.OpeningHours;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return StoreId;
        }
    }

    public class Store : Aggregate<StoreId>
    {
        private string _address;
        private Coordinate _coordinate;
        private string _openingHours;
        private Owner _owner;

        public Store(Manager manager, StoreId storeId, Owner owner) : base(storeId)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (owner == null) throw new ArgumentNullException("owner");

            _owner = owner;
            _address = null;
            _coordinate = null;
            _openingHours = "nach Vereinbarung";
        }

        protected Store()
        {
        }

        public virtual Owner Owner
        {
            get { return _owner; }
            protected set { _owner = value; }
        }

        public virtual string Address
        {
            get { return _address; }
            protected set { _address = value; }
        }

        public virtual Coordinate Coordinate
        {
            get { return _coordinate; }
            protected set { _coordinate = value; }
        }

        public virtual string OpeningHours
        {
            get { return _openingHours; }
            protected set { _openingHours = value; }
        }

        public virtual void ChangeAddress(Manager manager, string address)
        {
            if (Equals(_address, address))
                return;

            Address = address;
            ModifiedAtUtc = DateTime.UtcNow;

            EventPublisher.Publish(new AddressChanged(manager, Id, address));
        }

        public virtual void ChangeCoordinate(Manager manager, Coordinate coordinate)
        {
            if (Equals(_coordinate, coordinate))
                return;

            Coordinate = coordinate;
            ModifiedAtUtc = DateTime.UtcNow;

            EventPublisher.Publish(new CoordinateChanged(manager, Id, coordinate));
        }

        public virtual void ChangeOpeningHours(Manager manager, string openingHours)
        {
            if (Equals(_openingHours, openingHours))
                return;

            OpeningHours = openingHours;
            ModifiedAtUtc = DateTime.UtcNow;

            EventPublisher.Publish(new OpeningHoursChanged(manager, Id, openingHours));
        }
    }
}