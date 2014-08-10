namespace Phundus.Core.Shop.Orders.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrderRepository : IRepository<Order>
    {
        ICollection<Order> FindByUserId(int userId);
        
        IEnumerable<Order> FindByOrganizationId(int organizationId);
        IEnumerable<Order> FindByOrganizationId(int organizationId, OrderStatus status);

        int SumReservedAmount(int articleId);
        
    }
}