﻿namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
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

            var newTotal = _inInventory.GetTotalAsOf(asOfUtc) + change;

            if (change > 0)
                Apply(new QuantityInInventoryIncreased(OrganizationId, ArticleId, StockId, change,
                    newTotal, asOfUtc, comment));
            else
                Apply(new QuantityInInventoryDecreased(OrganizationId, ArticleId, StockId, change*-1,
                    newTotal, asOfUtc, comment));

            Apply(new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, new Period(asOfUtc), change));
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

            Apply(new StockAllocated(OrganizationId, ArticleId, StockId, allocationId, reservationId, period, quantity));

            Apply(CreateQuantityAvailableDecreased(period, quantity));            
        }

        protected void When(StockAllocated e)
        {
            _allocations.Add(new Allocation(new AllocationId(e.AllocationId), new ReservationId(e.ReservationId),
                e.Period, e.Quantity));
            
            _available.DecreaseAsOf(e.Quantity, e.FromUtc);
            _available.IncreaseAsOf(e.Quantity, e.ToUtc);
        }

        private QuantityAvailableChanged CreateQuantityAvailableChanged(Period period, int change)
        {
            return new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, period, change);
        }

        private QuantityAvailableChanged CreateQuantityAvailableIncreased(Period period, int change)
        {
            return new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, period, change);
        }

        private QuantityAvailableChanged CreateQuantityAvailableDecreased(Period period, int change)
        {
            return new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, period, change  * -1);
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
            var oldPeriod = allocation.Period;

            Apply(new AllocationPeriodChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId,
                allocation.Period, newPeriod));

            Apply(CreateQuantityAvailableIncreased(oldPeriod, allocation.Quantity));
            Apply(CreateQuantityAvailableDecreased(newPeriod, allocation.Quantity));
        }

        protected void When(AllocationPeriodChanged e)
        {
            var allocation = _allocations.Get(new AllocationId(e.AllocationId));

            allocation.ChangePeriod(e.NewPeriod);

            _available.IncreaseAsOf(allocation.Quantity, e.OldFromUtc);
            _available.DecreaseAsOf(allocation.Quantity, e.OldToUtc);

            _available.DecreaseAsOf(allocation.Quantity, e.NewFromUtc);
            _available.IncreaseAsOf(allocation.Quantity, e.NewToUtc);
        }

        public virtual void ChangeAllocationQuantity(AllocationId allocationId, int newQuantity)
        {
            var allocation = _allocations.Get(allocationId);
            var oldQuantity = allocation.Quantity;

            Apply(new AllocationQuantityChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId,
                allocation.Quantity, newQuantity));

            Apply(CreateQuantityAvailableChanged(allocation.Period, oldQuantity - newQuantity));
        }

        protected void When(AllocationQuantityChanged e)
        {
            var allocation = _allocations.Get(new AllocationId(e.AllocationId));

            allocation.ChangeQuantity(e.NewQuantity);

            _available.IncreaseAsOf(e.OldQuantity, allocation.Period.FromUtc);
            _available.DecreaseAsOf(e.OldQuantity, allocation.Period.ToUtc);

            _available.DecreaseAsOf(e.NewQuantity, allocation.Period.FromUtc);
            _available.IncreaseAsOf(e.NewQuantity, allocation.Period.ToUtc);
        }

        public virtual void DiscardAllocation(AllocationId allocationId)
        {
            var allocation = _allocations.Get(allocationId);
            Apply(new AllocationDiscarded(OrganizationId, ArticleId, StockId, allocationId, allocation.Period,
                allocation.Quantity));

            Apply(CreateQuantityAvailableIncreased(allocation.Period, allocation.Quantity));            
        }

        protected void When(AllocationDiscarded e)
        {
            _allocations.Remove(new AllocationId(e.AllocationId));
            _available.IncreaseAsOf(e.Quantity, e.FromUtc);
            _available.DecreaseAsOf(e.Quantity, e.ToUtc);
        }
    }
}