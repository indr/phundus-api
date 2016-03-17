namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class OpeningHoursChanged : DomainEvent
    {
        public OpeningHoursChanged(Manager initiator, StoreId storeId, string openingHours)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (openingHours == null) throw new ArgumentNullException("openingHours");

            Initiator = initiator.ToActor();
            StoreId = storeId.Id;
            OpeningHours = openingHours;
        }

        protected OpeningHoursChanged()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 3)]
        public string OpeningHours { get; protected set; }
    }
}