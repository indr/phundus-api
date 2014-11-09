namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Quantity : ValueObject
    {
        public Quantity(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}