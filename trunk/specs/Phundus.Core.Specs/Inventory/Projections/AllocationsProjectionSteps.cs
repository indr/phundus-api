namespace Phundus.Core.Specs.Inventory.Projections
{
    using System.Collections.Generic;
    using Contexts;
    using Core.Inventory.Application;
    using Core.Inventory.Application.Data;
    using Core.Inventory.Port.Adapter.Persistence.View;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AllocationsProjectionSteps
    {
        private readonly Container _container;
        private readonly StockContext _context;
        private IEnumerable<AllocationData> _allocations;

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
        
        [When(@"I ask for allocations")]
        public void WhenIAskForQuantitiesAvailableInStock()
        {
            _allocations = GetAllocationsQueryService().AllAllocationsByArticleId(_context.OrganizationId.Id, _context.ArticleId.Id);
        }

        [Then(@"allocation data")]
        public void ThenQuantitiesAvailableData(Table table)
        {
            table.CompareToSet(_allocations);
        }
    }
}