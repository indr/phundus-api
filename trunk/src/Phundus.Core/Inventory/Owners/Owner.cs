namespace Phundus.Inventory.Owners
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Stores.Model;

    public class Owner : ValueObject
    {
        public Owner(OwnerId ownerId, string name)
        {
            OwnerId = ownerId;
            Name = name;
        }

        protected Owner()
        {
        }

        public OwnerId OwnerId { get; private set; }

        public string Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OwnerId;
            yield return Name;
        }

        public virtual Store OpenStore(StoreId storeId)
        {
            return new Store(storeId, this);
        }
    }
}