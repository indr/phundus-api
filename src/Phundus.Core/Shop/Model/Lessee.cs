namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class Lessee : ValueObject
    {
        private string _city;
        private string _emailAddress;
        private string _firstName;
        private string _lastName;
        private LesseeId _lesseeId;
        private string _memberNumber;
        private string _mobilePhoneNumber;
        private string _postcode;
        private string _street;

        public Lessee(LesseeId lesseeId, string firstName, string lastName, string street, string postcode, string city,
            string emailAddress, string mobilePhoneNumber, string memberNumber)
        {
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");

            _lesseeId = lesseeId;
            _firstName = firstName;
            _lastName = lastName;
            _street = street;
            _postcode = postcode;
            _city = city;
            _emailAddress = emailAddress;
            _mobilePhoneNumber = mobilePhoneNumber;
            _memberNumber = memberNumber;
        }

        protected Lessee()
        {
        }

        public virtual LesseeId LesseeId
        {
            get { return _lesseeId; }
            protected set { _lesseeId = value; }
        }

        [DataMember(Order = 1)]
        protected virtual Guid LesseeGuid
        {
            get { return LesseeId.Id; }
            set { LesseeId = new LesseeId(value); }
        }

        [DataMember(Order = 2)]
        public virtual string FirstName
        {
            get { return _firstName; }
            protected set { _firstName = value; }
        }

        [DataMember(Order = 3)]
        public virtual string LastName
        {
            get { return _lastName; }
            protected set { _lastName = value; }
        }

        [DataMember(Order = 4)]
        public virtual string Street
        {
            get { return _street; }
            protected set { _street = value; }
        }

        [DataMember(Order = 5)]
        public virtual string Postcode
        {
            get { return _postcode; }
            protected set { _postcode = value; }
        }

        [DataMember(Order = 6)]
        public virtual string City
        {
            get { return _city; }
            protected set { _city = value; }
        }

        [DataMember(Order = 7)]
        public virtual string EmailAddress
        {
            get { return _emailAddress; }
            protected set { _emailAddress = value; }
        }

        [DataMember(Order = 8)]
        public virtual string MobilePhoneNumber
        {
            get { return _mobilePhoneNumber; }
            protected set { _mobilePhoneNumber = value; }
        }

        [DataMember(Order = 9)]
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
            yield return LesseeId;
        }
    }
}