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
        /// <param name="shortOrderId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrderDto GetById(CurrentUserId currentUserId, ShortOrderId shortOrderId);

        IEnumerable<OrderDto> Query(CurrentUserId currentUserId, ShortOrderId shortOrderId, UserId queryUserId, OrganizationId queryOrganizationId);
    }
}