namespace Phundus.Core.Shop.Orders.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrderRepository : IRepository<Order>
    {
        ICollection<Order> FindMy(int userId);
        int SumReservedAmount(int articleId);
        IEnumerable<Order> Find(int organizationId, OrderStatus status);
    }
}