namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemRemoved : DomainEvent
    {
        public OrderItemRemoved(Initiator initiator, OrderId orderId, OrderShortId orderShortId, OrderStatus orderStatus,
            decimal orderTotal, OrderEventLine orderLine)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (orderLine == null) throw new ArgumentNullException("orderLine");

            Initiator = initiator;
            OrderId = orderId.Id;
            OrderShortId = orderShortId.Id;
            OrderStatus = (int)orderStatus;
            OrderTotal = orderTotal;
            OrderLine = orderLine;
        }

        protected OrderItemRemoved()
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
        public OrderEventLine OrderLine { get; set; }
    }
}