namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using System;
    using System.Runtime.Serialization;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using Inventory.Domain.Model.Catalog;

    [DataContract]
    public class OrderItemAdded : DomainEvent
    {
        public OrderItemAdded(UserId initiatorId, OrganizationId organizationId, OrderId orderId, Guid orderItemId,
            ArticleId articleId, Period period, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");           
            AssertionConcern.AssertArgumentGreaterThan(quantity, 0, "Quantity must be greater than zero.");

            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderId = orderId.Id;
            OrderItemId = orderItemId;
            ArticleId = articleId.Id;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
            Quantity = quantity;
        }

        protected OrderItemAdded()
        {
            
        }

        public int InitiatorId { get; set; }
        public int OrganizationId { get; set; }
        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }

        public int ArticleId { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Quantity { get; set; }
    }
}