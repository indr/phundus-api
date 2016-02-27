namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemPeriodChanged : DomainEvent
    {
        public OrderItemPeriodChanged(Initiator initiator, OrderId orderId, OrderShortId orderShortId, int orderStatus,
            decimal orderTotal, Guid orderItemId, Period oldPeriod, Period newPeriod, OrderEventLine orderLine)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (oldPeriod == null) throw new ArgumentNullException("oldPeriod");
            if (newPeriod == null) throw new ArgumentNullException("newPeriod");
            if (orderLine == null) throw new ArgumentNullException("orderLine");

            Initiator = initiator;
            OrderId = orderId.Id;
            OrderShortId = orderShortId.Id;
            OrderStatus = orderStatus;
            OrderTotal = orderTotal;
            OrderItemId = orderItemId;
            OldPeriod = oldPeriod;
            NewPeriod = newPeriod;
            OrderLine = orderLine;
        }

        protected OrderItemPeriodChanged()
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
        public Period OldPeriod { get; set; }

        [DataMember(Order = 8)]
        public Period NewPeriod { get; set; }

        [DataMember(Order = 9)]
        public OrderEventLine OrderLine { get; set; }
    }
}