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
            Guid orderItemId, Period period)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be greater or less than zero.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
        }

        protected OrderItemPeriodChanged()
        {
        }

        public UserId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }
        public OrderId OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
    }
}