namespace Phundus.IdentityAccess.Model.Organizations
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrganizationContactDetailsChanged : DomainEvent
    {
        public OrganizationContactDetailsChanged(Initiator initiator, OrganizationId organizationId, string line1, string line2, string street,
            string postcode, string city, string phoneNumber, string emailAddress, string website)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");

            Initiator = initiator;
            OrganizationId = organizationId.Id;
            Line1 = line1;
            Line2 = line2;
            Street = street;
            Postcode = postcode;
            City = city;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Website = website;
        }

        protected OrganizationContactDetailsChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; set; }

        [DataMember(Order = 3)]
        public string Line1 { get; set; }

        [DataMember(Order = 4)]
        public string Line2 { get; set; }

        [DataMember(Order = 5)]
        public string Street { get; set; }

        [DataMember(Order = 6)]
        public string Postcode { get; set; }

        [DataMember(Order = 7)]
        public string City { get; set; }

        [DataMember(Order = 8)]
        public string PhoneNumber { get; set; }

        [DataMember(Order = 9)]
        public string EmailAddress { get; set; }

        [DataMember(Order = 10)]
        public string Website { get; set; }
    }
}