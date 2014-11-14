﻿namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class QuantityAsOf
    {
        public QuantityAsOf(int change, int total, DateTime asOfUtc)
        {
            Change = change;
            Total = total;
            AsOfUtc = asOfUtc;
        }

        public int Change { get; private set; }
        public int Total { get; private set; }
        public DateTime AsOfUtc { get; private set; }

        public void AddToTotal(int change)
        {
            Total += change;
        }
    }

    public class QuantitiesAsOf
    {
        private List<QuantityAsOf> _quantities = new List<QuantityAsOf>();

        private QuantityAsOf FindAtAsOfOrLatestBefore(DateTime asOfUtc)
        {
            return _quantities.Where(p => p.AsOfUtc <= asOfUtc).OrderByDescending(ks => ks.AsOfUtc).FirstOrDefault();
        }

        public void ChangeAsOf(int change, DateTime asOfUtc)
        {
            var atAsOfOrLatest = FindAtAsOfOrLatestBefore(asOfUtc);
            if ((atAsOfOrLatest != null) && (atAsOfOrLatest.AsOfUtc == asOfUtc))
            {
                throw new InvalidOperationException(
                    "Änderung des In-Inventory-Bestandes zum gleichen Zeitpunkt wird nicht unterstützt.");
            }

            var total = 0;
            if (atAsOfOrLatest != null)
                total = atAsOfOrLatest.Total;

            var quantityAsOf = new QuantityAsOf(change, total + change, asOfUtc);
            UpdateFutures(change, asOfUtc);

            _quantities.Add(quantityAsOf);

            _quantities = _quantities.OrderBy(ks => ks.AsOfUtc).ToList();
        }

        private void UpdateFutures(int change, DateTime asOfUtc)
        {
            foreach (var each in _quantities.Where(p => p.AsOfUtc > asOfUtc))
            {
                each.AddToTotal(change);
            }
        }

        public int GetTotalAsOf(DateTime asOfUtc)
        {
            var atAsOfOrLatest = FindAtAsOfOrLatestBefore(asOfUtc);

            if (atAsOfOrLatest == null)
                return 0;

            return atAsOfOrLatest.Total;
        }
    }

    public class Stock : EventSourcedRootEntity
    {
        private readonly QuantitiesAsOf _inInventory = new QuantitiesAsOf();

        public Stock(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            Apply(new StockCreated(organizationId.Id, articleId.Id, stockId.Id));
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

        public void ChangeQuantityInInventory(int change, DateTime asOfUtc, string comment)
        {
            if (change == 0)
                throw new ArgumentException("Change must be greater or less than 0", "change");

            var totalAsOf = _inInventory.GetTotalAsOf(asOfUtc);

            if (change > 0)
                Apply(new QuantityInInventoryIncreased(OrganizationId.Id, ArticleId.Id, StockId.Id, change,
                    totalAsOf + change, asOfUtc, comment));
            else if (change < 0)
                Apply(new QuantityInInventoryDecreased(OrganizationId.Id, ArticleId.Id, StockId.Id, change*-1,
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
    }
}