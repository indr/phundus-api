namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class DiscardAllocation : ICommand
    {
        public DiscardAllocation(OrganizationId organizationId, ArticleId articleId, StockId stockId, AllocationId allocationId)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");

            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            AllocationId = allocationId;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public StockId StockId { get; private set; }
        public AllocationId AllocationId { get; private set; }
    }

    public class DiscardAllocationHandler : IHandleCommand<DiscardAllocation>
    {
        public IStockRepository Repository { get; set; }

        public void Handle(DiscardAllocation command)
        {
            var stock = Repository.Get(command.OrganizationId, command.StockId);

            stock.DiscardAllocation(command.AllocationId);

            Repository.Save(stock);
        }
    }
}