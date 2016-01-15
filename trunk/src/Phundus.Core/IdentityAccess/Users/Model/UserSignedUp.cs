namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserSignedUp : DomainEvent
    {
        protected UserSignedUp()
        {
        }

        public UserSignedUp(UserGuid userGuid, string emailAddress, string password, string salt, string validationKey,
            string firstName, string lastName, string street, string postcode, string city, string mobilePhone)
        {
            if (userGuid == null) throw new ArgumentNullException("userGuid");
            UserGuid = userGuid.Id;
            EmailAddress = emailAddress;
            Password = password;
            Salt = salt;
            ValidationKey = validationKey;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Postcode = postcode;
            City = city;
            MobilePhone = mobilePhone;
        }

        [DataMember(Order = 1)]
        public Guid UserGuid { get; private set; }

        [DataMember(Order = 2)]
        public string EmailAddress { get; private set; }

        [DataMember(Order = 3)]
        public string Password { get; private set; }

        [DataMember(Order = 4)]
        public string Salt { get; private set; }

        [DataMember(Order = 5)]
        public string ValidationKey { get; private set; }

        [DataMember(Order = 6)]
        public string FirstName { get; private set; }

        [DataMember(Order = 7)]
        public string LastName { get; private set; }

        [DataMember(Order = 8)]
        public string Street { get; private set; }

        [DataMember(Order = 9)]
        public string Postcode { get; private set; }

        [DataMember(Order = 10)]
        public string City { get; private set; }

        [DataMember(Order = 11)]
        public string MobilePhone { get; private set; }

        [DataMember(Order = 12)]
        public int? JsNumber { get; private set; }
    }
}