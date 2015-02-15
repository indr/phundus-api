namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using Common.Extensions;
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
        private InInventory _inInventory = null;
        private Allocations _allocations = new Allocations();
        private Availabilities _availabilities = null;

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
            get { return _inInventory.QuantityAsOf; }
        }

        public ICollection<QuantityAsOf> QuantitiesAvailable
        {
            get { return _availabilities.Quantities; }
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

            _inInventory = new InInventory(OrganizationId, ArticleId, StockId);
            _availabilities = new Availabilities(OrganizationId, ArticleId, StockId);
        }

        public virtual void ChangeQuantityInInventory(Period period, int quantity, string comment)
        {
            Apply(_inInventory.Change(period, quantity, comment));

            CalculateAllocationStatuses(period);

            CalculateAvailabilities(period);
        }
        
        private void CalculateAllocationStatuses(Period period)
        {
            // TODO: Calculate allocation status in given period
            // Should be done like: Compare InInventory to Allocations. Set allocation status depending of InInventory.
            // Allocations.ChangeStatus(AllocationId, Status) returns AllocationStatusChanged-Event in case of change.
        }

        private void CalculateAvailabilities(Period period)
        {
            // TODO: Calculate availabilities in given period
            // Should be done like: InInventory - Allocations (Allocated)
            // Compare old availabilities to newly calculated. 
            // Apply events according to the difference.

            var availabilities = _inInventory.ComputeAvailabilities(_allocations);

            var events = _availabilities.GenerateMutatingEvents(availabilities);

            Apply(events);            
        }

        #region When-relays to sub entities
        protected void When(QuantityInInventoryIncreased e)
        {
            _inInventory.When(e);
        }

        protected void When(QuantityInInventoryDecreased e)
        {
            _inInventory.When(e);
        }

        protected void When(QuantityAvailableChanged e)
        {
            _availabilities.When(e);
        }
        #endregion

        public virtual void Allocate(AllocationId allocationId, ReservationId reservationId, Period period, int quantity)
        {
            Apply(new StockAllocated(OrganizationId, ArticleId, StockId, allocationId, reservationId, period, quantity));

            Apply(CreateQuantityAvailableDecreased(period, quantity));

            //Apply(new AllocationStatusChanged(OrganizationId, ArticleId, StockId, allocationId, AllocationStatus.Unknown,
            //    AllocationStatus.Unknown));
        }

        protected void When(StockAllocated e)
        {
            _allocations.Add(new Allocation(new AllocationId(e.AllocationId), new ReservationId(e.ReservationId),
                e.Period, e.Quantity));
        }

        public virtual void ChangeAllocationPeriod(AllocationId allocationId, Period newPeriod)
        {
            var allocation = _allocations.Get(allocationId);
            var oldPeriod = allocation.Period;

            Apply(CreateQuantityAvailableIncreased(oldPeriod, allocation.Quantity));
            Apply(CreateQuantityAvailableDecreased(newPeriod, allocation.Quantity));

            Apply(new AllocationPeriodChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId,
                allocation.Period, newPeriod));
        }

        protected void When(AllocationPeriodChanged e)
        {
            var allocation = _allocations.Get(new AllocationId(e.AllocationId));

            allocation.ChangePeriod(e.NewPeriod);
        }

        public virtual void ChangeAllocationQuantity(AllocationId allocationId, int newQuantity)
        {
            var allocation = _allocations.Get(allocationId);
            var oldQuantity = allocation.Quantity;

            Apply(CreateQuantityAvailableChanged(allocation.Period, oldQuantity - newQuantity));

            Apply(new AllocationQuantityChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId,
                allocation.Quantity, newQuantity));
        }

        protected void When(AllocationQuantityChanged e)
        {
            var allocation = _allocations.Get(new AllocationId(e.AllocationId));

            allocation.ChangeQuantity(e.NewQuantity);
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
        }

        

        protected void When(AllocationStatusChanged e)
        {
            var allocation = _allocations.Get(new AllocationId(e.AllocationId));

            allocation.Status = EnumHelper.Parse<AllocationStatus>(e.NewStatus);
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
            return new QuantityAvailableChanged(OrganizationId, ArticleId, StockId, period, change * -1);
        }
    }
}