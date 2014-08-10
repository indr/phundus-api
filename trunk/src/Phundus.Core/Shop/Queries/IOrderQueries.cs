namespace Phundus.Core.Shop.Queries
{
    using System.Collections.Generic;
    using Orders.Model;

    public interface IOrderQueries
    {
        OrderDto FindById(int orderId);
        IEnumerable<OrderDto> FindByOrganizationId(int organizationId, int currentUserId, OrderStatus status);
        IEnumerable<OrderDto> FindByUserId(int userId);
    }
}