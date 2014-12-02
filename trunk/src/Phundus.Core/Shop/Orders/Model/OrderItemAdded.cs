namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemAdded : DomainEvent
    {
        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }
    }

    [DataContract]
    public class OrderItemRemoved : DomainEvent
    {
    }
}