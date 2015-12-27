namespace Phundus.Core.Inventory.Stores.Model
{
    using System;
    using Common;
    using Ddd;
    using Owners;

    public class Store : Aggregate<StoreId>
    {
        private string _address;
        private Coordinate _coordinate;
        private string _openingHours;
        private Owner _owner;

        public Store(StoreId storeId, Owner owner) : base(storeId)
        {
            AssertionConcern.AssertArgumentNotNull(owner, "Owner must be provided.");

            _owner = owner;
            _address = null;
            _coordinate = null;
            _openingHours = null;
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

        public virtual void ChangeAddress(string address)
        {
            if (Equals(_address, address))
                return;

            Address = address;
            ModifiedAtUtc = DateTime.UtcNow;

            EventPublisher.Publish(new AddressChanged());
        }

        public virtual void ChangeCoordinate(Coordinate coordinate)
        {
            if (Equals(_coordinate, coordinate))
                return;

            Coordinate = coordinate;
            ModifiedAtUtc = DateTime.UtcNow;

            EventPublisher.Publish(new CoordinateChanged());
        }

        public virtual void ChangeOpeningHours(string openingHours)
        {
            if (Equals(_openingHours, openingHours))
                return;

            OpeningHours = openingHours;
            ModifiedAtUtc = DateTime.UtcNow;

            EventPublisher.Publish(new OpeningHoursChanged());
        }
    }
}