namespace Phundus.Core.Shop.Queries
{
    using System.Collections.Generic;
    using Orders.Model;

    public interface IOrderQueries
    {
        OrderDto FindById(int orderId, int userId);
        
        IEnumerable<OrderDto> FindByOrganizationId(int organizationId, int currentUserId);
        IEnumerable<OrderDto> FindByOrganizationId(int organizationId, int userId, OrderStatus status);

        IEnumerable<OrderDto> FindByUserId(int userId);

        OrderDto FindOrder(int orderId, int organizationId, int currentUserId);
    }
}