namespace Phundus.Shop.Orders.Model
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    /// <summary>
    /// Besitzer
    /// </summary>
    public class Owner : ValueObject
    {
        private string _name;
        private OwnerId _ownerId;

        public Owner(OwnerId ownerId, string name)
        {
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            _ownerId = ownerId;
            _name = name;
        }

        protected Owner()
        {
        }

        public virtual OwnerId OwnerId
        {
            get { return _ownerId; }
            protected set { _ownerId = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OwnerId;
        }
    }
}