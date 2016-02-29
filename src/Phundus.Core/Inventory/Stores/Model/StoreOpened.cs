namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class StoreOpened : DomainEvent
    {
        public StoreOpened(Manager manager, StoreId storeId, Owner owner)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (owner == null) throw new ArgumentNullException("owner");

            Manager = manager;
            StoreId = storeId.Id;
            Owner = owner;
        }

        protected StoreOpened()
        {
        }

        [DataMember(Order = 1)]
        public Manager Manager { get; protected set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 3)]
        public Owner Owner { get; protected set; }
    }
}