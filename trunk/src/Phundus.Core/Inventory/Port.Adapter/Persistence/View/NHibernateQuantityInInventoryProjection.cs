namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using System;
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Management;
    using NHibernate;
    using NHibernate.Linq;

    public class NHibernateQuantityInInventoryProjection : NHibernateProjectionBase<QuantityInInventoryData>,
        IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        private void Process(DomainEvent domainEvent)
        {
           // Fallback
        }

        private IQueryOver<QuantityInInventoryData, QuantityInInventoryData> QueryWhere(int organizationId,
            int articleId, string stockId)
        {
            return Query.Where(p => p.OrganizationId == organizationId).And(p => p.ArticleId == articleId)
                .And(p => p.StockId == stockId);
        }

        private void Process(QuantityInInventoryIncreased e)
        {
            var record = QueryWhere(e.OrganizationId, e.ArticleId, e.StockId).And(p => p.AsOfUtc == e.AsOfUtc).SingleOrDefault();
            if (record == null)
                record = new QuantityInInventoryData(e.OrganizationId, e.ArticleId, e.StockId);

            record.AsOfUtc = e.AsOfUtc;
            record.Total = e.Total;
            record.Change = e.Change;
            record.Comment = e.Comment;
            Save(record);

            UpdateFutures(e.OrganizationId, e.ArticleId, e.StockId, e.AsOfUtc, e.Change);
        }

        private void Process(QuantityInInventoryDecreased e)
        {
            var record = QueryWhere(e.OrganizationId, e.ArticleId, e.StockId).And(p => p.AsOfUtc == e.AsOfUtc).SingleOrDefault();
            if (record == null)
                record = new QuantityInInventoryData(e.OrganizationId, e.ArticleId, e.StockId);

            record.AsOfUtc = e.AsOfUtc;
            record.Total = e.Total;
            record.Change = e.Change*-1;
            record.Comment = e.Comment;
            Save(record);

            UpdateFutures(e.OrganizationId, e.ArticleId, e.StockId, e.AsOfUtc, e.Change*-1);
        }

        private void UpdateFutures(int organizationId, int articleId, string stockId, DateTime asOfUtc, int change)
        {
            var records = QueryWhere(organizationId, articleId, stockId).And(p => p.AsOfUtc > asOfUtc).List();
            foreach (var each in records)
            {
                each.Total += change;
                Save(each);
            }
        }
    }
}