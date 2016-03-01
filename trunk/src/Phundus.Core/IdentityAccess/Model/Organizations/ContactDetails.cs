namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Common.Domain.Model;    

    public class ContactDetails : ValueObject
    {
        private string _emailAddress;
        private string _line1;
        private string _line2;
        private string _street;
        private string _postcode;
        private string _city;
        private string _phoneNumber;
        private string _website;

        public ContactDetails(string line1, string line2, string street, string postcode, string city, string phoneNumber, string emailAddress, string website)
        {
            _line1 = line1;
            _line2 = line2;
            _street = street;
            _postcode = postcode;
            _city = city;
            _phoneNumber = phoneNumber;
            _emailAddress = emailAddress;
            _website = website;
        }

        protected ContactDetails()
        {
        }

        public virtual string Line1
        {
            get { return _line1; }
            protected set { _line1 = value; }
        }

        public virtual string Line2
        {
            get { return _line2; }
            protected set { _line2 = value; }
        }

        public virtual string Street
        {
            get { return _street; }
            protected set { _street = value; }
        }
        
        public virtual string Postcode
        {
            get { return _postcode; }
            protected set { _postcode = value; }
        }

        public virtual string City
        {
            get { return _city; }
            protected set { _city = value; }
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
            yield return Line1;
            yield return Line2;
            yield return Street;
            yield return Postcode;
            yield return City;
            yield return PhoneNumber;
            yield return EmailAddress;
            yield return Website;
        }
    }
}