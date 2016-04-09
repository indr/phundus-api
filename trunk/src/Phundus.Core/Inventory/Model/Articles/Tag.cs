namespace Phundus.Inventory.Model.Articles
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class Tag : ValueObject
    {
        public Tag(string name)
        {
            Name = name.ToFriendlyUrl();
        }

        public string Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}