namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemAdded : DomainEvent
    {
        public OrderItemAdded(Initiator initiator, OrderId orderId, OrderShortId orderShortId, int orderStatus,
            decimal orderTotal, OrderEventItem orderItem)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            Initiator = initiator;
            OrderId = orderId.Id;
            OrderShortId = orderShortId.Id;
            OrderStatus = orderStatus;
            OrderTotal = orderTotal;
            OrderItem = orderItem;
        }

        protected OrderItemAdded()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public Guid OrderId { get; set; }

        [DataMember(Order = 3)]
        public int OrderShortId { get; set; }

        [DataMember(Order = 4)]
        public int OrderStatus { get; set; }

        [DataMember(Order = 5)]
        public decimal OrderTotal { get; set; }

        [DataMember(Order = 6)]
        public OrderEventItem OrderItem { get; set; }
    }
}