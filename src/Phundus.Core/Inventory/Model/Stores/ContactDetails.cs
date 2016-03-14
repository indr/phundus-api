namespace Phundus.Inventory.Model.Stores
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class ContactDetails : ValueObject
    {
        public ContactDetails(string emailAddress, string phoneNumber, PostalAddress postalAddress)
        {
            AssertionConcern.AssertArgumentNotNull(postalAddress, "Postal address must be provided.");
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            PostalAddress = postalAddress;
        }

        private ContactDetails()
        {
        }

        public string EmailAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public PostalAddress PostalAddress { get; private set; }

        public static ContactDetails Empty
        {
            get { return new ContactDetails(); }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
            yield return PhoneNumber;
            yield return PostalAddress;
        }
    }
}