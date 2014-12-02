namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using System;
    using System.Runtime.Serialization;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    [DataContract]
    public class OrderItemRemoved : DomainEvent
    {
        public OrderItemRemoved(UserId initiatorId, OrganizationId organizationId, OrderId orderId, Guid orderItemId)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");

            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderId = orderId.Id;
            OrderItemId = orderItemId;
        }

        protected OrderItemRemoved()
        {
        }

        public int InitiatorId { get; protected set; }
        public int OrganizationId { get; protected set; }
        public int OrderId { get; protected set; }
        public Guid OrderItemId { get; protected set; }
    }
}