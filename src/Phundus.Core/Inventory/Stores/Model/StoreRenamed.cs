namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class StoreRenamed : DomainEvent
    {
        public StoreRenamed(Manager manager, StoreId storeId, string name)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (name == null) throw new ArgumentNullException("name");

            Manager = manager;
            StoreId = storeId.Id;
            Name = name;
        }

        [DataMember(Order = 1)]
        public Manager Manager { get; protected set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 3)]
        public string Name { get; protected set; }
    }
}