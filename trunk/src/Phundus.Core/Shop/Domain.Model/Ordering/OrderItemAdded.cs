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

        [DataMember(Order = 1)]
        public int InitiatorId { get; set; }

        [DataMember(Order = 2)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 3)]
        public int OrderId { get; set; }

        [DataMember(Order = 4)]
        public Guid OrderItemId { get; set; }

        [DataMember(Order = 5)]
        public int ArticleId { get; set; }

        [DataMember(Order = 6)]
        public DateTime FromUtc { get; set; }

        [DataMember(Order = 7)]
        public DateTime ToUtc { get; set; }

        public Period Period
        {
            get { return new Period(FromUtc, ToUtc); }
        }

        [DataMember(Order = 8)]
        public int Quantity { get; set; }
    }
}