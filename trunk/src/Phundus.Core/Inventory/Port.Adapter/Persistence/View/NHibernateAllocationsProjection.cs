namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using System;
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Management;
    using NHibernate;

    public class NHibernateAllocationsProjection : NHibernateProjectionBase<AllocationData>, IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        private void Process(DomainEvent e)
        {
            // Fallback
        }

        private IQueryOver<AllocationData, AllocationData> QueryWhere(int organizationId,
            int articleId, string stockId)
        {
            return Query.Where(p => p.OrganizationId == organizationId).And(p => p.ArticleId == articleId)
                .And(p => p.StockId == stockId);
        }

        private AllocationData QuerySingleOrDefault(int organizationId, int articleId, string stockId,
            Guid allocationId)
        {
            return QueryWhere(organizationId, articleId, stockId)
                .And(p => p.AllocationId == allocationId)
                .SingleOrDefault();
        }

        private void Process(StockAllocated e)
        {
            var record = new AllocationData(e.AllocationId, e.OrganizationId, e.ArticleId, e.StockId, e.ReservationId);
            record.FromUtc = e.FromUtc;
            record.ToUtc = e.ToUtc;
            record.Quantity = e.Quantity;
            record.AllocationStatus = e.AllocationStatus.ToString();
            Save(record);
        }

        private void Process(AllocationQuantityChanged e)
        {
            var record = QuerySingleOrDefault(e.OrganizationId, e.ArticleId, e.StockId, e.AllocationId);
            record.Quantity = e.NewQuantity;
            Save(record);
        }

        private void Process(AllocationPeriodChanged e)
        {
            var record = QuerySingleOrDefault(e.OrganizationId, e.ArticleId, e.StockId, e.AllocationId);
            record.Period = e.NewPeriod;
            Save(record);
        }

        private void Process(AllocationDiscarded e)
        {
            var record = QuerySingleOrDefault(e.OrganizationId, e.ArticleId, e.StockId, e.AllocationId);
            Delete(record);
        }

        private void Process(AllocationStatusChanged e)
        {
            var record = QuerySingleOrDefault(e.OrganizationId, e.ArticleId, e.StockId, e.AllocationId);
            record.AllocationStatus = e.NewStatus;
            Save(record);
        }
    }
}