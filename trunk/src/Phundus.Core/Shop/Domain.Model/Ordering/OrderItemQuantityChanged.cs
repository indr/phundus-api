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
    public class OrderItemQuantityChanged : DomainEvent
    {
        public OrderItemQuantityChanged(UserId initiatorId, OrganizationId organizationId, ArticleId articleId,
            OrderId orderId, Guid orderItemId, int oldQuantity, int newQuantity)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentGreaterThan(oldQuantity, 0, "Old quantity must be greater than zero.");
            AssertionConcern.AssertArgumentNotZero(newQuantity, "Quantity must be greater or less than zero.");

            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            OrderId = orderId.Id;
            OrderItemId = orderItemId;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
        }

        protected OrderItemQuantityChanged()
        {
        }

        [DataMember(Order = 1)]
        public int InitiatorId { get; set; }

        [DataMember(Order = 2)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 3)]
        public int ArticleId { get; set; }

        [DataMember(Order = 4)]
        public int OrderId { get; set; }

        [DataMember(Order = 5)]
        public Guid OrderItemId { get; set; }

        [DataMember(Order = 6)]
        public int OldQuantity { get; set; }

        [DataMember(Order = 7)]
        public int NewQuantity { get; set; }
    }
}