namespace Phundus.Core.Shop.Queries
{
    using System.Collections.Generic;

    public interface IOrderQueries
    {
        OrderDto SingleByOrderId(int orderId, int currentUserId);
        OrderDto SingleByOrderIdAndOrganizationId(int orderId, int organizationId, int currentUserId);

        IEnumerable<OrderDto> ManyByUserId(int userId);
        IEnumerable<OrderDto> ManyByOrganizationId(int organizationId, int currentUserId);
    }
}