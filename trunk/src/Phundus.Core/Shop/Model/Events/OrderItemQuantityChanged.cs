namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemQuantityChanged : DomainEvent
    {
        public OrderItemQuantityChanged(Initiator initiator, OrderId orderId, OrderShortId orderShortId, int orderStatus,
            decimal orderTotal, OrderItemId orderItemId, int oldQuantity, int newQuantity, OrderEventItem orderItem)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (orderItemId == null) throw new ArgumentNullException("orderItemId");
            if (orderItem == null) throw new ArgumentNullException("orderItem");

            Initiator = initiator;
            OrderId = orderId.Id;
            OrderShortId = orderShortId.Id;
            OrderStatus = orderStatus;
            OrderTotal = orderTotal;
            OrderItemId = orderItemId.Id;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
            OrderItem = orderItem;
        }

        protected OrderItemQuantityChanged()
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
        public int OldQuantity { get; set; }

        [DataMember(Order = 8)]
        public int NewQuantity { get; set; }

        [DataMember(Order = 9)]
        public OrderEventItem OrderItem { get; set; }
    }
}