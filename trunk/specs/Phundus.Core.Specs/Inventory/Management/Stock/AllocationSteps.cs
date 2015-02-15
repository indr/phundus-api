namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Domain.Model.Reservations;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AllocationStatusSteps
    {
        private Container _container;
        private StockContext _ctx;

        public AllocationStatusSteps(Container container, StockContext ctx)
        {
            _container = container;
            _ctx = ctx;
        }

        [Given(@"allocation status changed, allocation id (.*), new status Allocated")]
        public void GivenAllocationStatusChangedAllocationIdNewStatusUnavailable(Guid allocationId)
        {
            _ctx.PastEvents.Add(new AllocationStatusChanged(_ctx.OrganizationId, _ctx.ArticleId,
                _ctx.StockId, new AllocationId(allocationId), AllocationStatus.Unknown, AllocationStatus.Allocated));
        }

        [Then(@"allocation status changed, allocation id (.*), new status (.*)")]
        public void ThenAllocationStatusChangedAllocationIdNewStatusUnavailable(Guid allocationId, string newStatus)
        {
            var actual = _ctx.MutatingEvents.GetExpectedEventOnce<AllocationStatusChanged>();
            Assert.That(actual.AllocationId, Is.EqualTo(allocationId));
            Assert.That(actual.NewStatus, Is.EqualTo(newStatus));
        }
    }

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

        [When(@"allocate stock, allocation id (.*), reservation id (.*), from (.*) to (.*), quantity (.*)")]
        public void WhenAllocateItemForReservationFrom_To_(Guid allocationId, string reservationId,
            DateTime fromUtc, DateTime toUtc, int quantity)
        {
            _container.Resolve<AllocateStockHandler>().Handle(new AllocateStock(
                _context.OrganizationId, _context.ArticleId, _context.StockId, new AllocationId(allocationId),
                new ReservationId(reservationId), new Period(fromUtc, toUtc), quantity));
        }

        [Given(@"stock allocated, allocation id (.*), reservation id (.*), from (.*) to (.*), quantity (.*)")]
        public void GivenStockAllocatedAllocationIdReservationIdFromToQuantity(Guid allocationId, string reservationId,
            DateTime fromUtc, DateTime toUtc, int quantity)
        {
            _context.PastEvents.Add(new StockAllocated(_context.OrganizationId, _context.ArticleId, _context.StockId,
                new AllocationId(allocationId),
                new ReservationId(reservationId), new Period(fromUtc, toUtc), quantity));
        }

        [Then(@"stock allocated (.*)")]
        public void ThenStockAllocated(Guid allocationId)
        {
            var actual = _context.MutatingEvents.GetExpectedEventOnce<StockAllocated>();
            Assert.That(actual.AllocationId, Is.EqualTo(allocationId));
        }

        [When(@"change allocation period, allocation id (.*), new from (.*) to (.*)")]
        public void WhenChangeAllocationPeriodAllocationIdNewFromTo(Guid allocationId, DateTime newFromUtc,
            DateTime newToUtc)
        {
            _container.Resolve<ChangeAllocationPeriodHandler>().Handle(new ChangeAllocationPeriod(
                _context.OrganizationId, _context.ArticleId, _context.StockId, new AllocationId(allocationId),
                new Period(newFromUtc, newToUtc)));
        }

        [Given(@"allocation period changed, allocation id (.*), new from (.*) to (.*)")]
        public void WhenAllocationPeriodChangedAllocationIdNewPeriodFromTo(Guid allocationId, DateTime newFromUtc,
            DateTime newToUtc)
        {
            _context.PastEvents.Add(new AllocationPeriodChanged(_context.OrganizationId, _context.ArticleId,
                _context.StockId,
                new AllocationId(allocationId), Period.FromTodayToTomorrow, new Period(newFromUtc, newToUtc)));
        }

        [Then(@"allocation period changed, allocation id (.*), new from (.*) to (.*)")]
        public void ThenAllocationPeriodChangedAllocationIdNewFrom_To_(Guid allocationId, DateTime newFromUtc,
            DateTime newToUtc)
        {
            var actual = _context.MutatingEvents.GetExpectedEventOnce<AllocationPeriodChanged>();
            Assert.That(actual.AllocationId, Is.EqualTo(allocationId));
            Assert.That(actual.NewFromUtc, Is.EqualTo(newFromUtc));
            Assert.That(actual.NewToUtc, Is.EqualTo(newToUtc));
        }

        [Given(@"allocation quantity changed, allocation id (.*), new quantity (.*)")]
        public void GivenAllocationQuantityChangedAllocationIdQuantity(Guid allocationId, int newQuantity)
        {
            _context.PastEvents.Add(new AllocationQuantityChanged(_context.OrganizationId, _context.ArticleId,
                _context.StockId,
                new AllocationId(allocationId), 0, newQuantity));
        }

        [When(@"change allocation quantity, allocation id (.*), new quantity (.*)")]
        public void WhenChangeAllocationQuantityAllocationIdNewQuantity(Guid allocationId, int newQuantity)
        {
            _container.Resolve<ChangeAllocationQuantityHandler>().Handle(new ChangeAllocationQuantity(
                _context.OrganizationId, _context.ArticleId, _context.StockId, new AllocationId(allocationId),
                newQuantity));
        }

        [Then(@"allocation quantity changed, allocation id (.*), new quantity (.*)")]
        public void ThenAllocationQuantityChangedAllocationIdNewQuantity(Guid allocationId, int newQuantity)
        {
            var actual = _context.MutatingEvents.GetExpectedEventOnce<AllocationQuantityChanged>();
            Assert.That(actual.AllocationId, Is.EqualTo(allocationId));
            Assert.That(actual.NewQuantity, Is.EqualTo(newQuantity));
        }

        [Given(@"allocation discarded, allocation id (.*)")]
        public void GivenAllocationDiscardedAllocationId(Guid allocationId)
        {
            _context.PastEvents.Add(new AllocationDiscarded(_context.OrganizationId, _context.ArticleId,
                _context.StockId,
                new AllocationId(allocationId), Period.FromTodayToTomorrow, 1));
        }

        [When(@"discarding allocation allocation id (.*)")]
        public void WhenDiscardingAllocationAllocationId(Guid allocationId)
        {
            _container.Resolve<DiscardAllocationHandler>()
                .Handle(new DiscardAllocation(_context.OrganizationId, _context.ArticleId,
                    _context.StockId, new AllocationId(allocationId)));
        }

        [Then(@"allocation discarded allocation id (.*)")]
        public void ThenAllocationDiscardedAllocationId(Guid allcationId)
        {
            var actual = _context.MutatingEvents.GetExpectedEventOnce<AllocationDiscarded>();
            Assert.That(actual.AllocationId, Is.EqualTo(allcationId));
        }

        [Then(@"allocations")]
        public void ThenAllocations(Table table)
        {
            var actual = _context.Sut.Allocations.Select(
                s => new
                {
                    AllocationId = s.AllocationId.Id,
                    ReservationId = s.ReservationId.Id,
                    s.Period.FromUtc,
                    s.Period.ToUtc,
                    s.Quantity,
                    s.Status
                });
            table.CompareToSet(actual);
        }

        [Then(@"allocations is empty")]
        public void ThenAllocationsIsEmpty()
        {
            Assert.That(_context.Sut.Allocations, Is.Empty);
        }
    }
}