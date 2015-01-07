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

        public ArticleId ArticleId { get; private set; }

        public StockId StockId { get; private set; }

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
        }

        protected void When(QuantityInInventoryIncreased e)
        {
            _inInventory.ChangeAsOf(e.Change, e.AsOfUtc);
        }

        protected void When(QuantityInInventoryDecreased e)
        {
            _inInventory.ChangeAsOf(e.Change, e.AsOfUtc);
        }

        public virtual void Allocate(AllocationId allocationId, ReservationId reservationId, Period period, int quantity)
        {
            throw new NotImplementedException();
            //Apply(new StockAllocated(StockId, allocationId, reservationId, period, quantity));
        }

        public virtual void ChangeAllocationPeriod(AllocationId allocationId, Period newPeriod)
        {
            throw new NotImplementedException();
        }

        public virtual void ChangeAllocationQuantity(AllocationId allocationId, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public virtual void DiscardAllocation(AllocationId allocationId)
        {
            throw new NotImplementedException();
        }
    }

    public class StockAllocated : DomainEvent
    {
        public StockAllocated(StockId stockId, AllocationId allocationId, ReservationId reservationId, Period period, int quantity)
        {
            
        }
    }
}