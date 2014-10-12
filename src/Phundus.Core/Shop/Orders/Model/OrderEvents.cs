namespace Phundus.Core.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Ddd;

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