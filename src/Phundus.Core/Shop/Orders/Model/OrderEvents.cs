namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    [DataContract]
    public class OrderCreated : DomainEvent
    {
        public OrderCreated(OrderId orderId)
        {
            OrderId = orderId.Id;
        }

        protected OrderCreated()
        {
        }

        [DataMember(Order = 1)]
        public int OrderId { get; set; }
    }

    [DataContract]
    public class OrderRejected : DomainEvent
    {
        public OrderRejected(UserId initiatorId, OrganizationId organizationId, OrderId orderId,
            ICollection<Guid> orderItemIds)
        {
            OrderId = orderId.Id;
            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderItemIds = orderItemIds;
        }

        protected OrderRejected()
        {
        }

        [DataMember(Order = 1)]
        public int OrderId { get; set; }

        [DataMember(Order = 2)]
        public int InitiatorId { get; set; }

        [DataMember(Order = 3)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 4)]
        public ICollection<Guid> OrderItemIds { get; set; }
    }

    [DataContract]
    public class OrderApproved : DomainEvent
    {
        [DataMember(Order = 1)]
        public int OrderId { get; set; }
    }

    [DataContract]
    public class OrderClosed : DomainEvent
    {
        public OrderClosed(UserId initiatorId, OrganizationId organizationId, OrderId orderId,
            ICollection<Guid> orderItemIds)
        {
            OrderId = orderId.Id;
            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            OrderItemIds = orderItemIds;
        }

        protected OrderClosed()
        {
        }

        [DataMember(Order = 1)]
        public int OrderId { get; set; }

        [DataMember(Order = 2)]
        public int InitiatorId { get; set; }

        [DataMember(Order = 3)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 4)]
        public ICollection<Guid> OrderItemIds { get; set; }
    }
}