namespace Phundus.Core.Inventory.Stores.Model
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Ddd;

    public class StoreId : Identity<Guid>
    {
        public StoreId() : base(Guid.NewGuid())
        {
        }

        public StoreId(Guid value) : base(value)
        {
        }
    }

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

        public virtual void ChangeCoordinate(Coordinate coordinate)
        {
            if (Equals(_coordinate, coordinate))
                return;

            _coordinate = coordinate;

            EventPublisher.Publish(new CoordinateChanged());
        }
    }
}