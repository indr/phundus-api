namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using System;
    using System.Runtime.Serialization;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    [DataContract]
    public class OrderItemPeriodChanged : DomainEvent
    {
        public OrderItemPeriodChanged(UserId initiatorId, OrganizationId organizationId, OrderId orderId,
            Guid orderItemId, Period oldPeriod, Period newPeriod)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentNotNull(oldPeriod, "Old period must be provided.");
            AssertionConcern.AssertArgumentNotNull(newPeriod, "New period must be provided.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            OldFromUtc = oldPeriod.FromUtc;
            OldToUtc = oldPeriod.ToUtc;
            NewFromUtc = newPeriod.FromUtc;
            NewToUtc = newPeriod.ToUtc;
        }

        protected OrderItemPeriodChanged()
        {
        }

        public UserId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }
        public OrderId OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public DateTime OldFromUtc { get; set; }
        public DateTime OldToUtc { get; set; }
        public DateTime NewFromUtc { get; set; }
        public DateTime NewToUtc { get; set; }
    }
}