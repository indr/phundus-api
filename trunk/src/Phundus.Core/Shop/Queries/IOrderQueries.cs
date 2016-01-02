namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public interface IOrderQueries
    {
        [Obsolete]
        OrderDto SingleByOrderId(int orderId, int currentUserId);

        [Obsolete]
        OrderDto SingleByOrderIdAndOrganizationId(int orderId, Guid organizationId, int currentUserId);

        [Obsolete]
        IEnumerable<OrderDto> ManyByUserId(int userId);

        [Obsolete]
        IEnumerable<OrderDto> ManyByOrganizationId(Guid organizationId, int currentUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrderDto GetById(CurrentUserId currentUserId, OrderId orderId);

        IEnumerable<OrderDto> Query(CurrentUserId currentUserId, OrderId orderId, int? queryUserId, Guid? queryOrganizationId);
        
    }
}