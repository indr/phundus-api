namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class AllocationStatusChanged : DomainEvent
    {
        public AllocationStatusChanged(OrganizationId organizationId, ArticleId articleId, StockId stockId,
            AllocationId allocationId, AllocationStatus oldStatus, AllocationStatus newStatus)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(oldStatus, "Old status must be provided.");
            AssertionConcern.AssertArgumentNotNull(newStatus, "New status must be provided.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            StockId = stockId.Id;
            AllocationId = allocationId.Id;
            OldStatus = oldStatus.ToString();
            NewStatus = newStatus.ToString();
        }

        protected AllocationStatusChanged()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string StockId { get; protected set; }

        [DataMember(Order = 4)]
        public Guid AllocationId { get; protected set; }

        [DataMember(Order = 5)]
        public string OldStatus { get; protected set; }

        [DataMember(Order = 6)]
        public string NewStatus { get; protected set; }
    }
}