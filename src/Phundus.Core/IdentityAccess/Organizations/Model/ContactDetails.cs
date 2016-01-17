namespace Phundus.IdentityAccess.Organizations.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class ContactDetails : ValueObject
    {
        private string _emailAddress;
        private string _phoneNumber;
        private string _postAddress;
        private string _website;

        public ContactDetails(string postAddress, string phoneNumber, string emailAddress, string website)
        {
            _postAddress = postAddress;
            _phoneNumber = phoneNumber;
            _emailAddress = emailAddress;
            _website = website;
        }

        protected ContactDetails()
        {
        }

        public virtual string PostAddress
        {
            get { return _postAddress; }
            protected set { _postAddress = value; }
        }

        public virtual string PhoneNumber
        {
            get { return _phoneNumber; }
            protected set { _phoneNumber = value; }
        }

        public virtual string EmailAddress
        {
            get { return _emailAddress; }
            protected set { _emailAddress = value; }
        }

        public virtual string Website
        {
            get { return _website; }
            protected set { _website = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PostAddress;
            yield return PhoneNumber;
            yield return EmailAddress;
            yield return Website;
        }
    }
}