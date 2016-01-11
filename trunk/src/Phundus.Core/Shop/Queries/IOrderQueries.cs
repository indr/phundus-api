namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Phundus.Shop.Queries;

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

        IEnumerable<OrderDto> Query(CurrentUserId currentUserId, OrderId orderId, int? queryUserId, Guid? queryOrganizationId);
        
    }
}