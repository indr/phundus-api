namespace Phundus.Core.Inventory.Owners
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Stores.Model;

    public class Owner : ValueObject
    {
        private OwnerId _ownerId;
        private string _name;

        public Owner(OwnerId ownerId, string name)
        {
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
            yield return Name;
        }

        public virtual Store OpenStore()
        {
            return new Store(new StoreId(), this);
        }
    }
}