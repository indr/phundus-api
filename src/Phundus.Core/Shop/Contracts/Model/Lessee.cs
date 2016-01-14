namespace Phundus.Shop.Contracts.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Lessee : ValueObject
    {
        private string _city;
        private string _emailAddress;
        private string _firstName;
        private int _id;
        private string _lastName;
        private string _memberNumber;
        private string _mobilePhoneNumber;
        private string _postcode;
        private string _street;

        protected Lessee()
        {
        }

        public Lessee(int id, string firstName, string lastName, string street, string postcode, string city,
            string emailAddress, string mobilePhoneNumber, string memberNumber)
        {
            _id = id;
            _firstName = firstName;
            _lastName = lastName;
            _street = street;
            _postcode = postcode;
            _city = city;
            _emailAddress = emailAddress;
            _mobilePhoneNumber = mobilePhoneNumber;
            _memberNumber = memberNumber;
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual LesseeId LesseeId
        {
            get { return new LesseeId(_id); }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            protected set { _firstName = value; }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            protected set { _lastName = value; }
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

        public virtual string EmailAddress
        {
            get { return _emailAddress; }
            protected set { _emailAddress = value; }
        }

        public virtual string MobilePhoneNumber
        {
            get { return _mobilePhoneNumber; }
            protected set { _mobilePhoneNumber = value; }
        }

        public virtual string MemberNumber
        {
            get { return _memberNumber; }
            protected set { _memberNumber = value; }
        }

        public virtual string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}