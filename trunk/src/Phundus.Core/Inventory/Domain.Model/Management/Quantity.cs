namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Quantity : ValueObject
    {
        public Quantity(int quantity)
        {
            Value = quantity;
        }

        public int Value { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}