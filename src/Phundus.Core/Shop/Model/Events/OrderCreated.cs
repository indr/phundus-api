namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Shop.Model;

    [DataContract]
    public class OrderCreated : DomainEvent
    {
        public OrderCreated(Initiator initiator, OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee, OrderStatus orderStatus,
            decimal orderTotal, IList<OrderEventLine> orderLines = null)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (lessor == null) throw new ArgumentNullException("lessor");
            if (lessee == null) throw new ArgumentNullException("lessee");

            Initiator = initiator;
            OrderShortId = orderShortId.Id;
            OrderId = orderId.Id;
            Lessor = lessor;
            Lessee = lessee;
            OrderStatus = (int) orderStatus;
            OrderTotal = orderTotal;
            Lines = orderLines;
        }

        protected OrderCreated()
        {
        }

        [DataMember(Order = 1)]
        public int OrderShortId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrderId { get; protected set; }

        [DataMember(Order = 3)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 4)]
        public Lessor Lessor { get; protected set; }

        [DataMember(Order = 5)]
        public Lessee Lessee { get; protected set; }

        [DataMember(Order = 6)]
        public int OrderStatus { get; protected set; }

        [DataMember(Order = 7)]
        public decimal OrderTotal { get; protected set; }

        [DataMember(Order = 8)]
        public IList<OrderEventLine> Lines { get; protected set; }
    }
}