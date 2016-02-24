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

        public Lessor(LessorId lessorId, string name, bool doesPublicRental)
        {
            AssertionConcern.AssertArgumentNotNull(lessorId, "LessorId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            _lessorId = lessorId;
            _name = name;
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LessorId;
        }
    }
}