namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Owner : ValueObject
    {
        private Guid _ownerId;
        private string _name;

        public Owner(Guid ownerId, string name)
        {
            _ownerId = ownerId;
            _name = name;
        }

        protected Owner()
        {
        }

        public Guid OwnerId
        {
            get { return _ownerId; }
            protected set { _ownerId = value; }
        }

        public string Name
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