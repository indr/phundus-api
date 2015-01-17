namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Management;

    public class NHibernateAllocationsProjection : NHibernateProjectionBase<AllocationData>, IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic)domainEvent);
        }

        private void Process(DomainEvent e)
        {
            // Fallback
        }

        private void Process(StockAllocated e)
        {
            var record = new AllocationData(e.AllocationId, e.OrganizationId, e.ArticleId, e.StockId);
            record.FromUtc = e.FromUtc;
            record.ToUtc = e.ToUtc;
            record.Quantity = e.Quantity;
            record.Status = e.AllocationStatus.ToString();
            Save(record);
        }
    }
}