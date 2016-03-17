namespace Phundus.IdentityAccess.Model.Users
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserAddressChanged : DomainEvent
    {
        public UserAddressChanged(Initiator initiator, UserId userId, string firstName, string lastName, string street, string postcode,
            string city, string phoneNumber)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (userId == null) throw new ArgumentNullException("userId");

            Initiator = initiator.ToActor();
            UserId = userId.Id;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Postcode = postcode;
            City = city;
            PhoneNumber = phoneNumber;
        }

        protected UserAddressChanged()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid UserId { get; protected set; }

        [DataMember(Order = 3)]
        public string FirstName { get; protected set; }

        [DataMember(Order = 4)]
        public string LastName { get; protected set; }

        [DataMember(Order = 5)]
        public string Street { get; protected set; }

        [DataMember(Order = 6)]
        public string Postcode { get; protected set; }

        [DataMember(Order = 7)]
        public string City { get; protected set; }

        [DataMember(Order = 8)]
        public string PhoneNumber { get; protected set; }
    }
}