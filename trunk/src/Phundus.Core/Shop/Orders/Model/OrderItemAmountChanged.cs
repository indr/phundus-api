namespace Phundus.Core.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Ddd;

    [DataContract]
    public class OrderItemAmountChanged : DomainEvent
    {
    }
}