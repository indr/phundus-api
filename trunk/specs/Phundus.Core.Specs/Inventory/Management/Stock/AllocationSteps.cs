namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Common.Domain.Model;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Domain.Model.Reservations;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class AllocationSteps
    {
        private Container _container;
        private StockContext _context;
        private PastEvents _pastEvents;
        private MutatingEvents _mutatingEvents;

        public AllocationSteps(Container container, PastEvents pastEvents, MutatingEvents mutatingEvents, StockContext context)
        {
            _container = container;
            _pastEvents = pastEvents;
            _context = context;
            _mutatingEvents = mutatingEvents;
        }

        [When(@"allocate (.*) with id (.*) for reservation (.*) from (.*) to (.*)")]
        public void WhenAllocateItemForReservationFrom_To_(int quantity, Guid allocationId, string reservationId,
            DateTime fromUtc, DateTime toUtc)
        {
            _container.Resolve<AllocateStockHandler>().Handle(new AllocateStock(
                _context.OrganizationId, _context.StockId, new AllocationId(allocationId),
                new ReservationId(reservationId), new Period(fromUtc, toUtc), quantity));
        }

        [Then(@"stock allocated (.*), (New|Impossible|Promised)")]
        public void ThenStockAllocatedPromised(Guid allocationId, AllocationStatus status)
        {
            var evnt = _mutatingEvents.GetNextExpectedEvent<StockAllocated>();
            Assert.That(evnt.AllocationId, Is.EqualTo(allocationId));
            Assert.That(evnt.AllocationStatus, Is.EqualTo(status));
        }
    }
}