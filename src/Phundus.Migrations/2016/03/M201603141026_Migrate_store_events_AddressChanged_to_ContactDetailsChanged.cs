namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201603141026)]
    public class M201603141026_Migrate_store_events_AddressChanged_to_ContactDetailsChanged : EventMigrationBase
    {
        public enum OwnerType
        {
            Unknown,
            Organization,
            User
        }

        private readonly Regex _phoneNumberRegex = new Regex(@"^((Tel\.:? )|(Telefon ))?(\+?[ 0-9]+)$",
            RegexOptions.Multiline);

        private readonly Regex _postCodeCityRegex = new Regex(@"^(CH-)?([\d]{4}) ([\w ]+)$", RegexOptions.Multiline);

        private readonly Regex _emailAddressRegex = new Regex(@"^(.+\@.+)$", RegexOptions.Multiline);


        protected override void Migrate()
        {
            var storeOpeneds = FindStoredEvents<StoreOpened>(@"Phundus.Inventory.Stores.Model.StoreOpened, Phundus.Core");
            var addressChangeds =
                FindStoredEvents<AddressChanged>(@"Phundus.Inventory.Stores.Model.AddressChanged, Phundus.Core");

            foreach (var addressChanged in addressChangeds)
            {
                var storeOpened = storeOpeneds.SingleOrDefault(p => p.StoreId == addressChanged.StoreId);
                if (storeOpened == null)
                    throw new Exception("Could not find event store opened for store id " + addressChanged.StoreId);

                CreateContactDetailsChanged(storeOpened, addressChanged);
            }            
        }

        private void CreateContactDetailsChanged(StoreOpened storeOpened, AddressChanged addressChanged)
        {
            if (storeOpened.StoreId != addressChanged.StoreId)
                throw new Exception("StoreOpened.StoreId != AddressChanged.StoreId");
            var e = new ContactDetailsChanged();
            e.Manager = addressChanged.Manager;
            e.OwnerId = storeOpened.Owner.OwnerId.Id;
            e.StoreId = addressChanged.StoreId;
            if (!ParseAddress(e, addressChanged.Address))
                return;

            UpdateStoredEvent(addressChanged.EventGuid, e, e.StoreId, @"Phundus.Inventory.Model.Stores.ContactDetailsChanged, Phundus.Core");
        }

        private bool ParseAddress(ContactDetailsChanged e, string rawAddress)
        {
            var address = rawAddress.Split(new[] { "\n" }, StringSplitOptions.None).ToList();

            if (address.Count == 1)
            {
                address = address[0].Split(',').Select(p => p.Trim()).ToList();
                rawAddress = String.Join("\n", address);
            }

            if (address.Count == 0)
                return false;

            if (address.Count > 1 && address[1] == "")
                address.RemoveAt(1);
            if (address.Count > 2 && address[2] == "")
                address.RemoveAt(2);
            address.Remove("Niggi Studer");

            string postcode = "";
            string city = "";
            FindPostcodeAndCity(rawAddress, out postcode, out city);

            string phoneNumber = "";
            FindPhoneNumber(rawAddress, out phoneNumber);

            string emailAddress = "";
            FindEmailAddress(rawAddress, out emailAddress);

            if (address.Count <= 2)
            {
                e.Line1 = "";
                e.Line2 = "";
                e.Street = address[0];
                e.Postcode = postcode;
                e.City = city;
                e.PhoneNumber = phoneNumber;
                e.EmailAddress = emailAddress; 
                return true;
            }

            if (address.Count == 3)
            {
                e.Line1 = address[0];
                e.Line2 = "";
                e.Street = address[1];
                e.Postcode = postcode;
                e.City = city;
                e.PhoneNumber = phoneNumber;
                e.EmailAddress = emailAddress;
                return true;
            }

            if (address.Count >= 4 && (address[3] == "" || address[3] == emailAddress))
            {
                var line1 = address[0];
                var street = address[1];
                street = street == postcode + " " + city ? line1 : street;

                e.Line1 = line1 == street ? "" : line1;
                e.Line2 = "";
                e.Street = street;
                e.Postcode = postcode;
                e.City = city;
                e.PhoneNumber = phoneNumber;
                e.EmailAddress = emailAddress;
                return true;
            }
            if (address.Count >= 5 && address[4] == "")
            {
                var line1 = address[0];
                var line2 = address[1];
                var street = address[2];

                e.Line1 = line1;
                e.Line2 = line2;
                e.Street = street;
                e.Postcode = postcode;
                e.City = city;
                e.PhoneNumber = phoneNumber;
                e.EmailAddress = emailAddress;
                return true;
            }

            throw new Exception(rawAddress);
        }

        private void FindEmailAddress(string rawAddress, out string emailAddress)
        {
            emailAddress = "";
            var match = _emailAddressRegex.Match(rawAddress);
            if (!match.Success)
                return;

            emailAddress = match.Groups[1].Value;
        }

        private void FindPhoneNumber(string rawAddress, out string phoneNumber)
        {
            phoneNumber = "";
            var match = _phoneNumberRegex.Match(rawAddress);
            if (!match.Success)
                return;

            phoneNumber = match.Groups[4].Value;
        }

        private void FindPostcodeAndCity(string rawAddress, out string postcode, out string city)
        {
            var match = _postCodeCityRegex.Match(rawAddress);
            if (!match.Success)
                throw new Exception("Could not find postcode and city in " + rawAddress);

            postcode = match.Groups[2].Value;
            city = match.Groups[3].Value;
        }

        [DataContract]
        public class AddressChanged : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Manager Manager { get; protected set; }

            [DataMember(Order = 2)]
            public Guid StoreId { get; protected set; }

            [DataMember(Order = 3)]
            public string Address { get; protected set; }
        }

        [DataContract]
        public class ContactDetailsChanged : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Manager Manager { get; set; }

            [DataMember(Order = 2)]
            public Guid OwnerId { get; set; }

            [DataMember(Order = 3)]
            public Guid StoreId { get; set; }

            [DataMember(Order = 4)]
            public string EmailAddress { get; set; }

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

        [DataContract]
        public class Manager
        {
            public Manager(UserId userId, string emailAddress, string fullName)
            {
                if (userId == null) throw new ArgumentNullException("userId");
                if (emailAddress == null) throw new ArgumentNullException("emailAddress");
                if (fullName == null) throw new ArgumentNullException("fullName");

                UserId = userId;
                EmailAddress = emailAddress;
                FullName = fullName;
            }

            protected Manager()
            {
            }

            public UserId UserId { get; protected set; }

            [DataMember(Order = 1)]
            protected Guid UserGuid
            {
                get { return UserId.Id; }
                set { UserId = new UserId(value); }
            }

            [DataMember(Order = 2)]
            public string EmailAddress { get; protected set; }

            [DataMember(Order = 3)]
            public string FullName { get; protected set; }
        }

        [DataContract]
        public class Owner
        {
            private string _name;
            private OwnerId _ownerId;
            private OwnerType _type;

            public Owner(OwnerId ownerId, string name, OwnerType type)
            {
                if (ownerId == null) throw new ArgumentNullException("ownerId");
                if (name == null) throw new ArgumentNullException("name");
                if (type == OwnerType.Unknown) throw new ArgumentException("Owner type must not be unknown.", "type");

                _ownerId = ownerId;
                _name = name;
                _type = type;
            }

            protected Owner()
            {
            }

            public virtual OwnerId OwnerId
            {
                get { return _ownerId; }
                protected set { _ownerId = value; }
            }

            [DataMember(Order = 4)]
            protected virtual Guid OwnerGuid
            {
                get { return OwnerId.Id; }
                set { OwnerId = new OwnerId(value); }
            }

            [DataMember(Order = 2)]
            public virtual OwnerType Type
            {
                get { return _type; }
                protected set { _type = value; }
            }

            [DataMember(Order = 3)]
            public virtual string Name
            {
                get { return _name; }
                protected set { _name = value; }
            }
        }

        [DataContract]
        public class StoreOpened : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Manager Manager { get; protected set; }

            [DataMember(Order = 2)]
            public Guid StoreId { get; protected set; }

            [DataMember(Order = 3)]
            public Owner Owner { get; protected set; }
        }
    }
}