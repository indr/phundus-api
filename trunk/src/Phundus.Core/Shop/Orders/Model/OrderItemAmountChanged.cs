namespace Phundus.Core.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderItemAmountChanged : DomainEvent
    {
    }
}