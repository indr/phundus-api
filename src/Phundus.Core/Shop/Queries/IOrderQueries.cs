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
        /// <param name="orderShortId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrderDto GetById(CurrentUserId currentUserId, OrderShortId orderShortId);

        IEnumerable<OrderDto> Query(CurrentUserId currentUserId, OrderShortId orderShortId, UserId queryUserId, OrganizationId queryOrganizationId);
    }
}