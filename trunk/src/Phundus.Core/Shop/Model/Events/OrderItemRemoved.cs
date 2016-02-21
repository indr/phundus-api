namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemRemoved : DomainEvent
    {
        public OrderItemRemoved(Initiator initiator, OrderId orderId, ShortOrderId shortOrderId, int orderStatus,
            decimal orderTotal, OrderEventItem orderItem)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (shortOrderId == null) throw new ArgumentNullException("shortOrderId");
            if (orderItem == null) throw new ArgumentNullException("orderItem");
            Initiator = initiator;
            OrderId = orderId.Id;
            ShortOrderId = shortOrderId.Id;
            OrderStatus = orderStatus;
            OrderTotal = orderTotal;
            OrderItem = orderItem;
        }

        protected OrderItemRemoved()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public Guid OrderId { get; set; }

        [DataMember(Order = 3)]
        public int ShortOrderId { get; set; }

        [DataMember(Order = 4)]
        public int OrderStatus { get; set; }

        [DataMember(Order = 5)]
        public decimal OrderTotal { get; set; }

        [DataMember(Order = 6)]
        public OrderEventItem OrderItem { get; set; }
    }
}