namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Shop.Model;

    [DataContract]
    public class OrderItemRemoved : DomainEvent
    {
        public OrderItemRemoved(Manager manager, OrderId orderId, OrderShortId orderShortId, OrderStatus orderStatus,
            decimal orderTotal, OrderEventLine orderLine)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (orderLine == null) throw new ArgumentNullException("orderLine");

            Manager = manager.ToActor();
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
        public OrderEventLine OrderLine { get; protected set; }
    }
}