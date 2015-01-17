namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class AllocationQuantityChanged : DomainEvent
    {
        public AllocationQuantityChanged(OrganizationId organizationId, ArticleId articleId, StockId stockId,
            AllocationId allocationId, int oldQuantity, int newQuantity)
        {
            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            StockId = stockId.Id;
            AllocationId = allocationId.Id;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
        }

        protected AllocationQuantityChanged()
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
        public int OldQuantity { get; protected set; }

        [DataMember(Order = 6)]
        public int NewQuantity { get; protected set; }
    }
}