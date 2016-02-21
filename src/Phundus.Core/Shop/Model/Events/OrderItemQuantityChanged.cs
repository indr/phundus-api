namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemQuantityChanged : DomainEvent
    {
        public OrderItemQuantityChanged(Initiator initiator, OrderId orderId, ShortOrderId shortOrderId, int orderStatus,
            decimal orderTotal, Guid orderItemId, int oldQuantity, int newQuantity, OrderEventItem orderItem)
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
            OrderItemId = orderItemId;
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
        public int ShortOrderId { get; set; }

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