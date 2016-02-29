namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserSignedUp : DomainEvent
    {
        public UserSignedUp(UserId userId, UserShortId userShortId, string emailAddress, string password, string salt,
            string validationKey, string firstName, string lastName, string street, string postcode, string city,
            string phoneNumber)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (userShortId == null) throw new ArgumentNullException("userShortId");

            UserId = userId.Id;
            UserShortId = userShortId.Id;
            EmailAddress = emailAddress;
            Password = password;
            Salt = salt;
            ValidationKey = validationKey;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Postcode = postcode;
            City = city;
            PhoneNumber = phoneNumber;
        }

        protected UserSignedUp()
        {
        }

        [DataMember(Order = 1)]
        public int UserShortId { get; protected set; }

        [DataMember(Order = 13)]
        public Guid UserId { get; protected set; }

        [DataMember(Order = 2)]
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 3)]
        public string Password { get; protected set; }

        [DataMember(Order = 4)]
        public string Salt { get; protected set; }

        [DataMember(Order = 5)]
        public string ValidationKey { get; protected set; }

        [DataMember(Order = 6)]
        public string FirstName { get; protected set; }

        [DataMember(Order = 7)]
        public string LastName { get; protected set; }

        [DataMember(Order = 8)]
        public string Street { get; protected set; }

        [DataMember(Order = 9)]
        public string Postcode { get; protected set; }

        [DataMember(Order = 10)]
        public string City { get; protected set; }

        [DataMember(Order = 11)]
        public string PhoneNumber { get; protected set; }

        [DataMember(Order = 12)]
        public int? JsNumber { get; protected set; }
    }
}