namespace Phundus.Shop.Orders.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderCreated : DomainEvent
    {
        public OrderCreated(int orderId)
        {
            OrderId = orderId;
        }

        protected OrderCreated()
        {
        }

        [DataMember(Order = 1)]
        public int OrderId { get; protected set; }
    }
}