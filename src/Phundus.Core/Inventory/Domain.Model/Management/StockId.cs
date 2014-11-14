namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using Common;
    using Common.Domain.Model;

    public class StockId : Identity<string>
    {
        public StockId(string id) : base(id)
        {
            AssertionConcern.AssertArgumentNotEmpty(id, "Stock id must be provided.");
        }
    }
}