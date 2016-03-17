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

            Initiator = initiator.ToActor();
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
        public Guid LessorId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid OrderId { get; protected set; }

        [DataMember(Order = 4)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 5)]
        public Lessor Lessor { get; protected set; }

        [DataMember(Order = 6)]
        public Lessee Lessee { get; protected set; }

        [DataMember(Order = 7)]
        public int OrderStatus { get; protected set; }

        [DataMember(Order = 8)]
        public decimal OrderTotal { get; protected set; }

        [DataMember(Order = 9)]
        public IList<OrderEventLine> Items { get; protected set; }
    }
}