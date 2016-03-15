namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common;
    using Common.Domain.Model;

    [DataContract]
    public class Lessor : ValueObject
    {
        private bool _doesPublicRental;
        private LessorId _lessorId;
        private string _name;

        public Lessor(LessorId lessorId, string name, string postalAddress, string phoneNumber, string emailAddress,
            string website, bool doesPublicRental)
        {
            AssertionConcern.AssertArgumentNotNull(lessorId, "LessorId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");
            _lessorId = lessorId;
            _name = name;
            PostalAddress = postalAddress;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Website = website;
            _doesPublicRental = doesPublicRental;
        }

        protected Lessor()
        {
        }

        public virtual LessorId LessorId
        {
            get { return _lessorId; }
            protected set { _lessorId = value; }
        }

        [DataMember(Order = 1)]
        protected virtual Guid LessorGuid
        {
            get { return LessorId.Id; }
            set { LessorId = new LessorId(value); }
        }

        [DataMember(Order = 2)]
        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        [DataMember(Order = 3)]
        public virtual bool DoesPublicRental
        {
            get { return _doesPublicRental; }
            protected set { _doesPublicRental = value; }
        }

        [DataMember(Order = 4)]
        public string PostalAddress { get; set; }

        [DataMember(Order = 5)]
        public string PhoneNumber { get; set; }

        [DataMember(Order = 6)]
        public string EmailAddress { get; set; }

        [DataMember(Order = 7)]
        public string Website { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LessorId;
        }
    }
}