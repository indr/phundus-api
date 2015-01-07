namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Contexts;
    using TechTalk.SpecFlow;

    [Binding]
    public class AllocationSteps
    {
        private PastEvents _pastEvents;
        private StockContext _context;

        public AllocationSteps(PastEvents pastEvents, StockContext context)
        {
            _pastEvents = pastEvents;
            _context = context;
        }

        [When(@"allocate (.*) item for reservation (.*) from (.*) to (.*)")]
        public void WhenAllocateItemForReservationFrom_To_(int quantity, string reservationId, DateTime fromUtc, DateTime toUtc)
        {
            
        }

        [Then(@"article allocated")]
        public void ThenArticleAllocated()
        {
            ScenarioContext.Current.Pending();
        }

    }
}