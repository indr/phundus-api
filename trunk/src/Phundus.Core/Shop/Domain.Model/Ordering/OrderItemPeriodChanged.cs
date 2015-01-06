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
    public class OrderItemPeriodChanged : DomainEvent
    {
        public OrderItemPeriodChanged(UserId initiatorId, OrganizationId organizationId, OrderId orderId,
            Guid orderItemId, ArticleId articleId, Period oldPeriod, Period newPeriod)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(oldPeriod, "Old period must be provided.");
            AssertionConcern.AssertArgumentNotNull(newPeriod, "New period must be provided.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            ArticleId = articleId;
            OldFromUtc = oldPeriod.FromUtc;
            OldToUtc = oldPeriod.ToUtc;
            NewFromUtc = newPeriod.FromUtc;
            NewToUtc = newPeriod.ToUtc;
        }

        protected OrderItemPeriodChanged()
        {
        }

        public UserId InitiatorId { get; private set; }
        public OrganizationId OrganizationId { get; private set; }
        public OrderId OrderId { get; private set; }
        public Guid OrderItemId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public DateTime OldFromUtc { get; private set; }
        public DateTime OldToUtc { get; private set; }
        public DateTime NewFromUtc { get; private set; }
        public DateTime NewToUtc { get; private set; }        
    }
}