namespace Phundus.Inventory.Model.Stores
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ContactDetailsChanged : DomainEvent
    {
        public ContactDetailsChanged(Manager initiator, OwnerId ownerId, StoreId storeId, ContactDetails contactDetails)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (contactDetails == null) throw new ArgumentNullException("contactDetails");

            Initiator = initiator.ToActor();
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
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OwnerId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 4)]
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 5)]
        public string PhoneNumber { get; protected set; }

        [DataMember(Order = 6)]
        public string Line1 { get; protected set; }

        [DataMember(Order = 7)]
        public string Line2 { get; protected set; }

        [DataMember(Order = 8)]
        public string Street { get; protected set; }

        [DataMember(Order = 9)]
        public string Postcode { get; protected set; }

        [DataMember(Order = 10)]
        public string City { get; protected set; }
    }
}