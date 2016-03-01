namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class OpeningHoursChanged : DomainEvent
    {
        public OpeningHoursChanged(Manager manager, StoreId storeId, string openingHours)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (openingHours == null) throw new ArgumentNullException("openingHours");

            Manager = manager;
            StoreId = storeId.Id;
            OpeningHours = openingHours;
        }

        protected OpeningHoursChanged()
        {
        }

        [DataMember(Order = 1)]
        public Manager Manager { get; protected set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 3)]
        public string OpeningHours { get; protected set; }
    }
}