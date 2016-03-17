namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class StoreRenamed : DomainEvent
    {
        public StoreRenamed(Manager initiator, StoreId storeId, string name)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (name == null) throw new ArgumentNullException("name");

            Initiator = initiator.ToActor();
            StoreId = storeId.Id;
            Name = name;
        }

        protected StoreRenamed()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 3)]
        public string Name { get; protected set; }
    }
}