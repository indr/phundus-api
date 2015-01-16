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
        private readonly Container _container;
        private readonly StockContext _context;

        public AllocationSteps(Container container, StockContext context)
        {
            _container = container;
            _context = context;
        }

        [When(@"allocate (.*) with id (.*) for reservation (.*) from (.*) to (.*)")]
        public void WhenAllocateItemForReservationFrom_To_(int quantity, Guid allocationId, string reservationId,
            DateTime fromUtc, DateTime toUtc)
        {
            _container.Resolve<AllocateStockHandler>().Handle(new AllocateStock(
                _context.OrganizationId, _context.ArticleId, _context.StockId, new AllocationId(allocationId),
                new ReservationId(reservationId), new Period(fromUtc, toUtc), quantity));
        }

        [Then(@"stock allocated (.*), (Unavailable|Allocated)")]
        public void ThenStockAllocatedPromised(Guid allocationId, AllocationStatus status)
        {
            var actual = _context.MutatingEvents.GetNextExpectedEvent<StockAllocated>();
            Assert.That(actual.AllocationId, Is.EqualTo(allocationId));
            Assert.That(actual.AllocationStatus, Is.EqualTo(status));
        }
    }
}