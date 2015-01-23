namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common;
    using Common.Domain.Model;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    [DataContract]
    public class OrderClosed : DomainEvent
    {
        public OrderClosed(UserId initiatorId, OrganizationId organizationId, OrderId orderId,
            ICollection<Guid> orderItemIds)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemIds, "Order item ids must be provided.");

            OrderId = orderId.Id;
            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderItemIds = orderItemIds;
        }

        protected OrderClosed()
        {
            OrderItemIds = new List<Guid>();
        }

        [DataMember(Order = 1)]
        public int OrderId { get; protected set; }

        [DataMember(Order = 2)]
        public int InitiatorId { get; protected set; }

        [DataMember(Order = 3)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 4)]
        public ICollection<Guid> OrderItemIds { get; protected set; }
    }
}