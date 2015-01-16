namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class ChangeAllocationQuantity : ICommand
    {
        public ChangeAllocationQuantity(OrganizationId organizationId, ArticleId articleId, StockId stockId, AllocationId allocationId, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");
            AssertionConcern.AssertArgumentNotZero(quantity, "Quantity must be greater or less than zero.");

            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            AllocationId = allocationId;
            Quantity = quantity;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public StockId StockId { get; private set; }
        public AllocationId AllocationId { get; private set; }
        public int Quantity { get; private set; }
    }

    public class ChangeAllocationQuantityHandler : AllocationHandlerBase, IHandleCommand<ChangeAllocationQuantity>
    {
        public ChangeAllocationQuantityHandler(IStockRepository stockRepository, IArticleRepository articleRepository) : base(stockRepository, articleRepository)
        {
        }

        public void Handle(ChangeAllocationQuantity command)
        {
            var stock = GetStock(command.OrganizationId, command.ArticleId, command.StockId);

            stock.ChangeAllocationQuantity(command.AllocationId, command.Quantity);

            StockRepository.Save(stock);
        }
    }
}