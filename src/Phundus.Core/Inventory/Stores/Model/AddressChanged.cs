namespace Phundus.Inventory.Stores.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class AddressChanged : DomainEvent
    {
        public AddressChanged(Manager manager, StoreId storeId, string address)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (address == null) throw new ArgumentNullException("address");

            Manager = manager;
            StoreId = storeId.Id;
            Address = address;
        }

        protected AddressChanged()
        {
        }

        [DataMember(Order = 1)]
        public Manager Manager { get; set; }

        [DataMember(Order = 2)]
        public Guid StoreId { get; set; }

        [DataMember(Order = 3)]
        public string Address { get; set; }
    }
}