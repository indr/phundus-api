namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common.Domain.Model;

    public class OrderId : Identity<int>
    {
        public OrderId(int id) : base(id)
        {
        }
    }
}