namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Shop.Model;

    [DataContract]
    public class OrderCreated : DomainEvent
    {
        public OrderCreated(Initiator initiator, OrderId orderId, OrderShortId orderShortId, Lessor lessor,
            Lessee lessee)
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
        }

        protected OrderCreated()
        {
        }

        [DataMember(Order = 1)]
        public int OrderShortId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrderId { get; set; }

        [DataMember(Order = 3)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 4)]
        public Lessor Lessor { get; set; }

        [DataMember(Order = 5)]
        public Lessee Lessee { get; set; }
    }
}