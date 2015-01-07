namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using Contexts;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using TechTalk.SpecFlow;

    [Binding]
    public class StockSteps
    {
        private readonly PastEvents _pastEvents;
        private readonly StockContext _context;

        public StockSteps(PastEvents pastEvents, StockContext context)
        {
            _pastEvents = pastEvents;
            _context = context;
        }

        [Given(@"stock created (.*), article (.*), organization (.*)")]
        public void StockCreated(string stockId, int articleId, int organizationId)
        {
            _context.OrganizationId = new OrganizationId(organizationId);
            _context.ArticleId = new ArticleId(articleId);
            _context.StockId = new StockId(stockId);
            _pastEvents.Add(new StockCreated(_context.OrganizationId, _context.ArticleId, _context.StockId));
        }
    }
}