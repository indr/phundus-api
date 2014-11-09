namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using Common.Domain.Model;

    public class StockId : Identity<string>
    {
        public StockId() : base(Guid.NewGuid().ToString())
        {
        }

        public StockId(string id) : base(id)
        {
        }
    }
}