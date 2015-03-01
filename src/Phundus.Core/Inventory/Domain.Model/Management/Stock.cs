namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using Common.Extensions;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Itenso.TimePeriod;
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
        private readonly QuantityInInventory _quantityInInventory = new QuantityInInventory();
        private readonly QuantityAllocated _quantityAllocated = new QuantityAllocated();
        private readonly QuantityAvailable _quantityAvailable = new QuantityAvailable();

        public Stock(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");

            SetMutatingEventsOnSubEntities();

            Apply(new StockCreated(organizationId, articleId, stockId));
        }

        public Stock(IEnumerable<IDomainEvent> eventStream, long streamVersion) : base(eventStream, streamVersion)
        {
            SetMutatingEventsOnSubEntities();
        }

        private void SetMutatingEventsOnSubEntities()
        {
            _quantityInInventory.MutatingEvents = MutatingEvents;
            _quantityAllocated.MutatingEvents = MutatingEvents;
            _quantityAvailable.MutatingEvents = MutatingEvents;
        }

        public OrganizationId OrganizationId { get; private set; }

        public StockId StockId { get; private set; }

        public ArticleId ArticleId { get; private set; }

        public ICollection<QuantityAsOf> QuantitiesInInventory
        {
            get { return _quantityInInventory.QuantityAsOf; }
        }

        public ICollection<QuantityAsOf> QuantitiesAvailable
        {
            get { return _quantityAvailable.Quantities; }
        }

        public ICollection<Allocation> Allocations
        {
            get { return _quantityAllocated.Items; }
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

            _quantityInInventory.When(e);
            _quantityAllocated.When(e);
            _quantityAvailable.When(e);
        }

        public virtual void ChangeQuantityInInventory(Period period, int quantity, string comment)
        {
            // InInventory-Änderung
            _quantityInInventory.Change(period, quantity, comment);

            // Führt zur Veränderung von Allocation-Statusen, weil
            // - wenn InInventory erhöht wurde, dann kann eine Allokation erfüllt werden
            // - wenn InInventory verringert wurde, dann können Allokationen nicht mehr erfüllt werden
            CalculateAllocationStatuses(period);

            // Führt zur Veränderung der Verfügbarkeit, weil
            // - InInventory verändert wurde
            // - Allocation-Status verändert sein könnte
            CalculateQuantityAvailable(period);
        }
        
        private void CalculateAllocationStatuses(Period period)
        {
            // TODO: Calculate allocation status in given period
            // Should be done like: Compare InInventory to Allocations. Set allocation status depending of InInventory.
            // Allocations.ChangeStatus(AllocationId, Status) returns AllocationStatusChanged-Event in case of change.
        }

        private void CalculateQuantityAvailable(Period period)
        {
            _quantityAvailable.Change(period, 0);
        }

        #region When-relays to sub entities
        protected void When(QuantityInInventoryIncreased e)
        {
            _quantityInInventory.When(e);
        }

        protected void When(QuantityInInventoryDecreased e)
        {
            _quantityInInventory.When(e);
        }

        protected void When(QuantityAvailableChanged e)
        {
            _quantityAvailable.When(e);
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
            _quantityAllocated.Add(new Allocation(new AllocationId(e.AllocationId), new ReservationId(e.ReservationId),
                e.Period, e.Quantity));
        }

        public virtual void ChangeAllocationPeriod(AllocationId allocationId, Period newPeriod)
        {
            var allocation = _quantityAllocated.Get(allocationId);
            var oldPeriod = allocation.Period;

            Apply(CreateQuantityAvailableIncreased(oldPeriod, allocation.Quantity));
            Apply(CreateQuantityAvailableDecreased(newPeriod, allocation.Quantity));

            Apply(new AllocationPeriodChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId,
                allocation.Period, newPeriod));
        }

        protected void When(AllocationPeriodChanged e)
        {
            var allocation = _quantityAllocated.Get(new AllocationId(e.AllocationId));

            allocation.ChangePeriod(e.NewPeriod);
        }

        public virtual void ChangeAllocationQuantity(AllocationId allocationId, int newQuantity)
        {
            var allocation = _quantityAllocated.Get(allocationId);
            var oldQuantity = allocation.Quantity;

            Apply(CreateQuantityAvailableChanged(allocation.Period, oldQuantity - newQuantity));

            Apply(new AllocationQuantityChanged(OrganizationId, ArticleId, StockId, allocation.AllocationId,
                allocation.Quantity, newQuantity));
        }

        protected void When(AllocationQuantityChanged e)
        {
            var allocation = _quantityAllocated.Get(new AllocationId(e.AllocationId));

            allocation.ChangeQuantity(e.NewQuantity);
        }

        public virtual void DiscardAllocation(AllocationId allocationId)
        {
            var allocation = _quantityAllocated.Get(allocationId);
            Apply(new AllocationDiscarded(OrganizationId, ArticleId, StockId, allocationId, allocation.Period,
                allocation.Quantity));

            Apply(CreateQuantityAvailableIncreased(allocation.Period, allocation.Quantity));
        }

        protected void When(AllocationDiscarded e)
        {
            _quantityAllocated.Remove(new AllocationId(e.AllocationId));
        }

        

        protected void When(AllocationStatusChanged e)
        {
            var allocation = _quantityAllocated.Get(new AllocationId(e.AllocationId));

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