namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using System;
    using System.Runtime.Serialization;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    [DataContract]
    public class OrderItemQuantityChanged : DomainEvent
    {
        public OrderItemQuantityChanged(UserId initiatorId, OrganizationId organizationId, OrderId orderId,
            Guid orderItemId, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentNotZero(quantity, "Quantity must be greater or less than zero.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            Quantity = quantity;
        }

        protected OrderItemQuantityChanged()
        {
        }

        public UserId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }
        public OrderId OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
    }
}