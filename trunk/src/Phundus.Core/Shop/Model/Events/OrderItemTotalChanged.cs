namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemTotalChanged : DomainEvent
    {
        public OrderItemTotalChanged(Initiator initiator, OrderId orderId, OrderShortId orderShortId, int orderStatus,
            decimal orderTotal, OrderLineId orderLineId, decimal oldItemTotal, decimal newItemTotal, OrderEventItem orderItem)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (orderLineId == null) throw new ArgumentNullException("orderLineId");
            if (orderItem == null) throw new ArgumentNullException("orderItem");

            Initiator = initiator;
            OrderId = orderId.Id;
            OrderShortId = orderShortId.Id;
            OrderStatus = orderStatus;
            OrderTotal = orderTotal;
            OrderItemId = orderLineId.Id;
            OldItemTotal = oldItemTotal;
            NewItemTotal = newItemTotal;
            OrderItem = orderItem;
        }

        protected OrderItemTotalChanged()
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
        public Guid OrderItemId { get; set; }

        [DataMember(Order = 7)]
        public decimal OldItemTotal { get; set; }

        [DataMember(Order = 8)]
        public decimal NewItemTotal { get; set; }

        [DataMember(Order = 9)]
        public OrderEventItem OrderItem { get; set; }
    }
}