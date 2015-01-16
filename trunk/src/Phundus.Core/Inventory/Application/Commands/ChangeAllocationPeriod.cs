namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class ChangeAllocationPeriod : ICommand
    {
        public ChangeAllocationPeriod(OrganizationId organizationId, ArticleId articleId, StockId stockId, AllocationId allocationId, Period period)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");

            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            AllocationId = allocationId;
            Period = period;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public StockId StockId { get; private set; }
        public AllocationId AllocationId { get; private set; }
        public Period Period { get; private set; }
    }

    public class ChangeAllocationPeriodHandler : AllocationHandlerBase, IHandleCommand<ChangeAllocationPeriod>
    {
        public ChangeAllocationPeriodHandler(IStockRepository stockRepository, IArticleRepository articleRepository) : base(stockRepository, articleRepository)
        {
        }

        public void Handle(ChangeAllocationPeriod command)
        {
            var stock = GetStock(command.OrganizationId, command.ArticleId, command.StockId);

            stock.ChangeAllocationPeriod(command.AllocationId, command.Period);

            StockRepository.Save(stock);
        }
    }
}