namespace Phundus.Inventory.Model.Stores
{
    using Common.Domain.Model;

    public class ContactDetails : Entity
    {
        public ContactDetails(string emailAddress, string phoneNumber, PostalAddress postalAddress)
        {
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            PostalAddress = postalAddress;
        }

        public string EmailAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public PostalAddress PostalAddress { get; private set; }        
    }
}