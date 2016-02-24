namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class CoordinateChanged : DomainEvent
    {
        public CoordinateChanged(Manager manager, StoreId storeId, Coordinate coordinate)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (coordinate == null) throw new ArgumentNullException("coordinate");

            Manager = manager;
            StoreId = storeId.Id;
            Latitude = coordinate.Latitude;
            Longitude = coordinate.Longitude;
        }

        protected CoordinateChanged()
        {
        }

        [DataMember(Order = 1)]
        public Manager Manager { get; set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; set; }

        [DataMember(Order = 3)]
        public decimal Latitude { get; set; }

        [DataMember(Order = 4)]
        public decimal Longitude { get; set; }
    }
}