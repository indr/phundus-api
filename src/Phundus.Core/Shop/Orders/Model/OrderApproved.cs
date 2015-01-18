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
    public class OrderApproved : DomainEvent
    {
        public OrderApproved(UserId initiatorId, OrganizationId organizationId, OrderId orderId,
            ICollection<Guid> orderItemIds)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemIds, "Order item ids must be provided.");

            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderId = orderId.Id;
            OrderItemIds = orderItemIds;
        }

        protected OrderApproved()
        {
        }

        [DataMember(Order = 1)]
        public int InitiatorId { get; protected set; }

        [DataMember(Order = 2)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public int OrderId { get; protected set; }

        [DataMember(Order = 4)]
        public ICollection<Guid> OrderItemIds { get; protected set; }
    }
}