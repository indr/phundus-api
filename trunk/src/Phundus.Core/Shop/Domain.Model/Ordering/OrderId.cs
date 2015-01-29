namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using System;
    using Common.Domain.Model;

    public class OrderId : Identity<int>
    {
        public OrderId(int id) : base(id)
        {
        }
    }

    public class OrderItemId : Identity<Guid>
    {
        public OrderItemId(Guid id) : base(id)
        {
        }
    }
}