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
    public class OrderItemRemoved : DomainEvent
    {
        public OrderItemRemoved(UserId initiatorId, OrganizationId organizationId, OrderId orderId, Guid orderItemId,
            ArticleId articleId)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");

            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderId = orderId.Id;
            OrderItemId = orderItemId;
            ArticleId = articleId.Id;
        }


        protected OrderItemRemoved()
        {
        }

        [DataMember(Order = 1)]
        public int InitiatorId { get; protected set; }

        [DataMember(Order = 2)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public int OrderId { get; protected set; }

        [DataMember(Order = 4)]
        public Guid OrderItemId { get; protected set; }

        [DataMember(Order = 5)]
        public int ArticleId { get; set; }
    }
}