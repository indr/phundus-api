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

            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderId = orderId.Id;
            OrderItemId = orderItemId;
            ArticleId = articleId.Id;
            OldFromUtc = oldPeriod.FromUtc;
            OldToUtc = oldPeriod.ToUtc;
            NewFromUtc = newPeriod.FromUtc;
            NewToUtc = newPeriod.ToUtc;
        }

        protected OrderItemPeriodChanged()
        {
        }

        [DataMember(Order = 1)]
        public int InitiatorId { get; private set; }

        [DataMember(Order = 2)]
        public int OrganizationId { get; private set; }

        [DataMember(Order = 3)]
        public int OrderId { get; private set; }

        [DataMember(Order = 4)]
        public Guid OrderItemId { get; private set; }

        [DataMember(Order = 5)]
        public int ArticleId { get; private set; }

        [DataMember(Order = 6)]
        public DateTime OldFromUtc { get; private set; }

        [DataMember(Order = 7)]
        public DateTime OldToUtc { get; private set; }

        [DataMember(Order = 8)]
        public DateTime NewFromUtc { get; private set; }

        [DataMember(Order = 9)]
        public DateTime NewToUtc { get; private set; }        
    }
}