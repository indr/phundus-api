namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Shop.Model;

    [DataContract]
    public class OrderPlaced : DomainEvent
    {
        public OrderPlaced(Initiator initiator, OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee,
            OrderStatus orderStatus, decimal orderTotal, IList<OrderEventLine> items)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (lessor == null) throw new ArgumentNullException("lessor");
            if (lessee == null) throw new ArgumentNullException("lessee");
            if (items == null) throw new ArgumentNullException("items");

            Initiator = initiator;
            OrderId = orderId.Id;
            OrderShortId = orderShortId.Id;
            Lessor = lessor;
            Lessee = lessee;
            OrderStatus = (int)orderStatus;
            OrderTotal = orderTotal;
            LessorId = lessor.LessorId.Id;
            Items = items;
        }

        protected OrderPlaced()
        {
        }

        [DataMember(Order = 1)]
        public int OrderShortId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid LessorId { get; set; }

        [DataMember(Order = 3)]
        public Guid OrderId { get; set; }

        [DataMember(Order = 4)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 5)]
        public Lessor Lessor { get; set; }

        [DataMember(Order = 6)]
        public Lessee Lessee { get; set; }

        [DataMember(Order = 7)]
        public int OrderStatus { get; set; }

        [DataMember(Order = 8)]
        public decimal OrderTotal { get; set; }

        [DataMember(Order = 9)]
        public IList<OrderEventLine> Items { get; set; }
    }
}