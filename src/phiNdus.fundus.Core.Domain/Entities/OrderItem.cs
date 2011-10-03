namespace phiNdus.fundus.Core.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Order Order { get; set; }
    }
}