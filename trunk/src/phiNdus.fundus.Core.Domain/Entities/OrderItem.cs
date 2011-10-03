namespace phiNdus.fundus.Core.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public virtual Order Order { get; set; }
    }
}