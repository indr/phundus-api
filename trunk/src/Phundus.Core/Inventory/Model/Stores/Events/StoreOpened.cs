namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class StoreOpened : DomainEvent
    {
        public StoreOpened(Manager initiator, StoreId storeId, Owner owner)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (owner == null) throw new ArgumentNullException("owner");

            Initiator = initiator.ToActor();
            StoreId = storeId.Id;
            Owner = owner;
        }

        protected StoreOpened()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 3)]
        public Owner Owner { get; protected set; }
    }
}