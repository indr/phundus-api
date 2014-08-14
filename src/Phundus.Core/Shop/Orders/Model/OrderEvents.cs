namespace Phundus.Core.Shop.Orders.Model
{
    using Ddd;

    public class OrderCreated : DomainEvent
    {
        public int OrderId { get; set; }
    }

    public class OrderRejected : DomainEvent
    {
        public int OrderId { get; set; }
    }

    public class OrderApproved : DomainEvent
    {
        public int OrderId { get; set; }
    }

    public class OrderClosed : DomainEvent
    {
        public int OrderId { get; set; }
    }
}