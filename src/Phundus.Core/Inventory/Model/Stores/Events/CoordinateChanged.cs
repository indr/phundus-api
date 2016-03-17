namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;
    using Inventory.Model.Stores;

    [DataContract]
    public class CoordinateChanged : DomainEvent
    {
        public CoordinateChanged(Manager initiator, StoreId storeId, Coordinate coordinate)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (coordinate == null) throw new ArgumentNullException("coordinate");

            Initiator = initiator.ToActor();
            StoreId = storeId.Id;
            Latitude = coordinate.Latitude;
            Longitude = coordinate.Longitude;
        }

        protected CoordinateChanged()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 3)]
        public decimal Latitude { get; protected set; }

        [DataMember(Order = 4)]
        public decimal Longitude { get; protected set; }
    }
}