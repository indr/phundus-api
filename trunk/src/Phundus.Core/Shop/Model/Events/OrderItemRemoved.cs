namespace Phundus.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemRemoved : DomainEvent
    {
    }
}