namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Shop.Model;

    [DataContract]
    public class OrderItemQuantityChanged : DomainEvent
    {
        public OrderItemQuantityChanged(Manager manager, OrderId orderId, OrderShortId orderShortId, int orderStatus,
            decimal orderTotal, OrderLineId orderLineId, int oldQuantity, int newQuantity, OrderEventLine orderLine)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (orderLineId == null) throw new ArgumentNullException("orderLineId");
            if (orderLine == null) throw new ArgumentNullException("orderLine");

            Manager = manager.ToActor();
            OrderId = orderId.Id;
            OrderShortId = orderShortId.Id;
            OrderStatus = orderStatus;
            OrderTotal = orderTotal;
            OrderItemId = orderLineId.Id;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
            OrderLine = orderLine;
        }

        protected OrderItemQuantityChanged()
        {
        }

        [DataMember(Order = 1)]
        public Actor Manager { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrderId { get; protected set; }

        [DataMember(Order = 3)]
        public int OrderShortId { get; protected set; }

        [DataMember(Order = 4)]
        public int OrderStatus { get; protected set; }

        [DataMember(Order = 5)]
        public decimal OrderTotal { get; protected set; }

        [DataMember(Order = 6)]
        public Guid OrderItemId { get; protected set; }

        [DataMember(Order = 7)]
        public int OldQuantity { get; protected set; }

        [DataMember(Order = 8)]
        public int NewQuantity { get; protected set; }

        [DataMember(Order = 9)]
        public OrderEventLine OrderLine { get; protected set; }
    }
}