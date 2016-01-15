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
        //[Obsolete]
        //OrderDto GetById(CurrentUserId currentUserId, OrderId orderId);

        OrderDto GetById(UserGuid initiatorGuid, OrderId orderId);

        IEnumerable<OrderDto> Query(UserGuid currentUserId, OrderId orderId, int? queryUserId, Guid? queryOrganizationId);
        
    }
}