namespace Phundus.Core.Inventory.Application.Data
{
    using System;

    public class QuantityInInventoryData
    {
        public virtual Guid Id { get; set; }
        public virtual int ConcurrencyVersion { get; set; }

        public virtual int Total { get; set; }
        public virtual int Change { get; set; }
        public virtual DateTime AsOfUtc { get; set; }
    }
}