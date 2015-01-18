namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Reservations;

    /// <summary>
    /// Quantities:
    /// - In-Inventory: Bruttobestand / Buchhalterischer Bestand
    /// - Allocated: Reservierter bestand, z.B. durch Bestellung
    /// - Available: Verfügbar für Reservierungen
    /// 
    /// - In-Stock / On-Hand: Lagerbestand
    /// 
    /// Berechnung:
    /// - In-Inventory: Fix
    /// - Allocated : Fix
    /// - 
    /// - Available: In-Inventory - Allocated
    /// </summary>
    public class Stock : EventSourcedRootEntity
    {
        private readonly Allocations _allocations = new Allocations();
        private readonly QuantitiesAsOf _available = new QuantitiesAsOf();
        private readonly QuantitiesAsOf _inInventory = new QuantitiesAsOf();

        public Stock(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");

            Apply(new StockCreated(organizationId, articleId, stockId));
        }

        public Stock(IEnumerable<IDomainEvent> eventStream, long streamVersion) : base(eventStream, streamVersion)
        {
        }

        public OrganizationId OrganizationId { get; private set; }

        public StockId StockId { get; private set; }

        public ArticleId ArticleId { get; private set; }

        public ICollection<QuantityAsOf> QuantitiesInInventory
        {
            get { return _inInventory.Quantities; }
        }

        public ICollection<QuantityAsOf> QuantitiesAvailable
        {
            get { return _available.Quantities; }
        }

        public ICollection<Allocation> Allocations
        {
            get { return _allocations.Items; }
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return StockId;
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        protected void When(StockCreated e)
        {
            OrganizationId = new OrganizationId(e.OrganizationId);
            ArticleId = new ArticleId(e.ArticleId);
            StockId = new StockId(e.StockId);
        }

        public virtual void ChangeQuantityInInventory(int change, DateTime asOfUtc, string comment)
        {
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than 0.");
            AssertionConcern.AssertArgumentNotEmpty(asOfUtc, "As of utc must be provided.");

            var totalAsOf = _inInventory.GetTotalAsOf(asOfUtc);

            if (change > 0)
                Apply(new QuantityInInventoryIncreased(OrganizationId, ArticleId, StockId, change,
                    totalAsOf + change, asOfUtc, comment));
            else if (change < 0)
                Apply(new QuantityInInventoryDecreased(OrganizationId, ArticleId, StockId, change*-1,
                    totalAsOf + change, asOfUtc, comment));

            Apply(new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, change, totalAsOf + change, asOfUtc));
        }

        protected void When(QuantityInInventoryIncreased e)
        {
            _inInventory.IncreaseAsOf(e.Change, e.AsOfUtc);
            _available.IncreaseAsOf(e.Change, e.AsOfUtc);
        }

        protected void When(QuantityInInventoryDecreased e)
        {
            _inInventory.DecreaseAsOf(e.Change, e.AsOfUtc);
            _available.DecreaseAsOf(e.Change, e.AsOfUtc);
        }

        protected void When(QuantityAvailableChanged e)
        {
            // QuantityAvailableChanged is not used for event sourcing. Availability is based on in-inventory and allocations.
        }

        public virtual void Allocate(AllocationId allocationId, ReservationId reservationId, Period period, int quantity)
        {
            var status = CalculateAllocationStatus(period, quantity);

            Apply(new StockAllocated(OrganizationId, ArticleId, StockId, allocationId, reservationId, period, quantity,
                status));

            _available.DecreaseAsOf(quantity, period.FromUtc);
            _available.IncreaseAsOf(quantity, period.ToUtc);

            Apply(CreateQuantityAvailableDecreased(quantity, period.FromUtc));
            Apply(CreateQuantityAvailableIncreased(quantity, period.ToUtc));
        }

        protected void When(StockAllocated e)
        {
            _allocations.Add(new Allocation(new AllocationId(e.AllocationId), new ReservationId(e.ReservationId), e.Period, e.Quantity));
        }
        
        private QuantityAvailableChanged CreateQuantityAvailableIncreased(int change, DateTime asOfUtc)
        {
            var total = _available.GetTotalAsOf(asOfUtc);
            return new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, change, total, asOfUtc);
        }

        private QuantityAvailableChanged CreateQuantityAvailableDecreased(int change, DateTime asOfUtc)
        {
            var total = _available.GetTotalAsOf(asOfUtc);
            return new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, change*-1, total, asOfUtc);
        }

        private AllocationStatus CalculateAllocationStatus(Period period, int quantity)
        {
            var hasQuantity = _available.HasQuantityInPeriod(period, quantity);
            if (hasQuantity)
                return AllocationStatus.Allocated;
            return AllocationStatus.Unavailable;
        }

        

        public virtual void ChangeAllocationPeriod(AllocationId allocationId, Period newPeriod)
        {
            var allocation = _allocations.Get(allocationId);

            Apply(new AllocationPeriodChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId, allocation.Period, newPeriod));
        }

        protected void When(AllocationPeriodChanged e)
        {
            var allocation = _allocations.Get(new AllocationId(e.AllocationId));

            allocation.ChangePeriod(e.NewPeriod);
        }

        public virtual void ChangeAllocationQuantity(AllocationId allocationId, int newQuantity)
        {
            var allocation = _allocations.Get(allocationId);

            Apply(new AllocationQuantityChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId, allocation.Quantity, newQuantity));
        }

        protected void When(AllocationQuantityChanged e)
        {
            var allocation = _allocations.Get(new AllocationId(e.AllocationId));

            allocation.ChangeQuantity(e.NewQuantity);
        }

        public virtual void DiscardAllocation(AllocationId allocationId)
        {
            Apply(new AllocationDiscarded(OrganizationId, ArticleId, StockId, allocationId));
        }

        protected void When(AllocationDiscarded e)
        {
            
        }
    }
}