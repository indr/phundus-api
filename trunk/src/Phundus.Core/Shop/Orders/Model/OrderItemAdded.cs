namespace Phundus.Core.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Ddd;

    [DataContract]
    public class OrderItemAdded : DomainEvent
    {
    }

    [DataContract]
    public class OrderItemRemoved : DomainEvent
    {
    }
}