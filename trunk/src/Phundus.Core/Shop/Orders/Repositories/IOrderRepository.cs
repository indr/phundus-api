namespace Phundus.Core.Shop.Orders.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrderRepository : IRepository<Order>
    {
        Order GetById(int orderId);

        ICollection<Order> FindByUserId(int userId);
        
        IEnumerable<Order> FindByOrganizationId(int organizationId);
        IEnumerable<Order> FindByOrganizationId(int organizationId, OrderStatus status);

        int SumReservedAmount(int articleId);

        new int Add(Order entity);
    }
}