namespace Phundus.Shop.Orders.Model
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class Lessor : ValueObject
    {
        private LessorId _lessorId;
        private string _name;

        public Lessor(LessorId lessorId, string name)
        {
            AssertionConcern.AssertArgumentNotNull(lessorId, "LessorId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            _lessorId = lessorId;
            _name = name;
        }

        protected Lessor()
        {
        }

        public virtual LessorId LessorId
        {
            get { return _lessorId; }
            protected set { _lessorId = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LessorId;
        }
    }
}