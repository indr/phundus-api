namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;

    /// <summary>
    /// Vermieter
    /// </summary>
    public class Lessor : ValueObject
    {
        private Guid _lessorId;
        private string _name;

        public Lessor(Guid lessorId, string name)
        {
            _lessorId = lessorId;
            _name = name;
        }

        protected Lessor()
        {
        }

        public Guid LessorId
        {
            get { return _lessorId; }
            private set { _lessorId = value; }
        }

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LessorId;
        }
    }
}