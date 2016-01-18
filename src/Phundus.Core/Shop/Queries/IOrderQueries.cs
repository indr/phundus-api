namespace Phundus.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public interface IOrderQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrderDto GetById(CurrentUserId currentUserId, OrderId orderId);

        IEnumerable<OrderDto> Query(CurrentUserId currentUserId, OrderId orderId, UserGuid queryUserId, OrganizationGuid queryOrganizationId);
    }
}