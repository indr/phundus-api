namespace Phundus.Shop.Orders.Model
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
        private Guid _lessorGuid;
        private string _name;

        public Lessor(LessorId lessorId, string name, bool doesPublicRental)
        {
            AssertionConcern.AssertArgumentNotNull(lessorId, "LessorId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            _lessorGuid = lessorId.Id;
            _name = name;
            _doesPublicRental = doesPublicRental;
        }

        protected Lessor()
        {
        }

        [DataMember(Order = 1)]
        public virtual Guid LessorGuid
        {
            get { return _lessorGuid; }
            protected set { _lessorGuid = value; }
        }

        public virtual LessorId LessorId
        {
            get { return new LessorId(LessorGuid); }
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LessorId;
        }
    }
}