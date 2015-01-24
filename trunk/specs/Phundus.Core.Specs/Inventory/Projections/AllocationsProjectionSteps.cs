namespace Phundus.Core.Specs.Inventory.Projections
{
    using Contexts;
    using Core.Inventory.Application;
    using Core.Inventory.Port.Adapter.Persistence.View;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AllocationsProjectionSteps
    {
        private readonly Container _container;
        private readonly StockContext _context;

        public AllocationsProjectionSteps(Container container, StockContext context)
        {
            _container = container;
            _context = context;
        }

        private AllocationsQueryService GetAllocationsQueryService()
        {
            _context.PastEvents.ProjectTo<NHibernateAllocationsProjection>();

            return _container.Resolve<AllocationsQueryService>();
        }

        [Then(@"all allocations by article id")]
        public void ThenQuantitiesAvailableData(Table table)
        {
            var allocations = GetAllocationsQueryService()
                .AllAllocationsByArticleId(_context.OrganizationId.Id, _context.ArticleId.Id);
            table.CompareToSet(allocations);
        }
    }
}