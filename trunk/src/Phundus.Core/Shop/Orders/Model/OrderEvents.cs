namespace Phundus.Core.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Domain.Model.Ordering;

    [DataContract]
    public class OrderCreated : DomainEvent
    {
        public OrderCreated(OrderId orderId)
        {
            OrderId = orderId.Id;
        }

        protected OrderCreated()
        {
        }

        [DataMember(Order = 1)]
        public int OrderId { get; set; }
    }

    [DataContract]
    public class OrderRejected : DomainEvent
    {
        [DataMember(Order = 1)]
        public int OrderId { get; set; }
    }

    [DataContract]
    public class OrderApproved : DomainEvent
    {
        [DataMember(Order = 1)]
        public int OrderId { get; set; }
    }

    [DataContract]
    public class OrderClosed : DomainEvent
    {
        [DataMember(Order = 1)]
        public int OrderId { get; set; }
    }
}