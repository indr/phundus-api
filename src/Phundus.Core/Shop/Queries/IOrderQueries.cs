namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IOrderQueries
    {
        OrderDto SingleByOrderId(int orderId, int currentUserId);
        OrderDto SingleByOrderIdAndOrganizationId(int orderId, Guid organizationId, int currentUserId);

        IEnumerable<OrderDto> ManyByUserId(int userId);
        IEnumerable<OrderDto> ManyByOrganizationId(Guid organizationId, int currentUserId);
        
        IEnumerable<OrderDto> Query(int currentUserId, int? orderId, int? queryUserId, Guid? queryOrganizationId);
    }
}