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

        public virtual OwnerId OwnerId { get; private set; }
        public virtual StoreId StoreId { get; private set; }
        public virtual Coordinate Coordinate { get; private set; }
        public virtual string OpeningHours { get; private set; }        
        public virtual string Name { get; private set; }
        public virtual ContactDetails ContactDetails { get; private set; }

        protected void When(StoreOpened e)
        {
            OwnerId = e.Owner.OwnerId;
            StoreId = new StoreId(e.StoreId);
            ContactDetails = ContactDetails.Empty;
        }


        public virtual void ChangeContactDetails(Manager manager, ContactDetails contactDetails)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");

            if (Equals(ContactDetails, contactDetails))
                return;

            Apply(new ContactDetailsChanged(manager, OwnerId, StoreId, contactDetails));
        }

        protected void When(ContactDetailsChanged e)
        {
            ContactDetails = new ContactDetails(e.EmailAddress, e.PhoneNumber,
                new PostalAddress(e.Line1, e.Line2, e.Street, e.Postcode, e.City));
        }


        public virtual void ChangeCoordinate(Manager manager, Coordinate coordinate)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotNull(coordinate, "Coordinate must be provided.");

            if (Equals(Coordinate, coordinate))
                return;

            Apply(new CoordinateChanged(manager, StoreId, coordinate));
        }

        protected void When(CoordinateChanged e)
        {
            Coordinate = new Coordinate(e.Latitude, e.Longitude);
        }


        public virtual void ChangeOpeningHours(Manager manager, string openingHours)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotNull(openingHours, "Opening hours must be provided.");

            if (Equals(OpeningHours, openingHours))
                return;

            Apply(new OpeningHoursChanged(manager, StoreId, openingHours));
        }

        protected void When(OpeningHoursChanged e)
        {
            OpeningHours = e.OpeningHours;
        }


        public virtual void Rename(Manager manager, string name)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            if (Equals(Name, name))
                return;

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