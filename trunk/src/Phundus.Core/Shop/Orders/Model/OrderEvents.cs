namespace Phundus.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderCreated : DomainEvent
    {
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