namespace Phundus.Inventory.Model.Articles
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class Tag : ValueObject
    {
        public Tag(string name)
        {
            name = name.Replace(' ', '-');
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Name = name;
        }

        public string Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}