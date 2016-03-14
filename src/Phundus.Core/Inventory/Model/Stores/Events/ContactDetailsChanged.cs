namespace Phundus.Inventory.Model.Stores
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ContactDetailsChanged : DomainEvent
    {
        public ContactDetailsChanged(Manager manager, OwnerId ownerId, StoreId storeId, ContactDetails contactDetails)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (contactDetails == null) throw new ArgumentNullException("contactDetails");
            Manager = manager;
            OwnerId = ownerId.Id;
            StoreId = storeId.Id;
            EmailAddress = contactDetails.EmailAddress;
            PhoneNumber = contactDetails.PhoneNumber;
            Line1 = contactDetails.PostalAddress.Line1;
            Line2 = contactDetails.PostalAddress.Line2;
            Street = contactDetails.PostalAddress.Street;
            Postcode = contactDetails.PostalAddress.Postcode;
            City = contactDetails.PostalAddress.City;
        }

        protected ContactDetailsChanged()
        {
        }

        [DataMember(Order = 1)]
        public Manager Manager { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OwnerId { get; set; }

        [DataMember(Order = 3)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 4)]
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 5)]
        public string PhoneNumber { get; set; }

        [DataMember(Order = 6)]
        public string Line1 { get; set; }

        [DataMember(Order = 7)]
        public string Line2 { get; set; }

        [DataMember(Order = 8)]
        public string Street { get; set; }

        [DataMember(Order = 9)]
        public string Postcode { get; set; }

        [DataMember(Order = 10)]
        public string City { get; set; }
    }
}