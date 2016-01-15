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
        /// <param name="currentUserGuid"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrderDto GetById(CurrentUserGuid currentUserGuid, OrderId orderId);

        IEnumerable<OrderDto> Query(CurrentUserGuid currentUserGuid, OrderId orderId, UserGuid queryUserId, OrganizationGuid queryOrganizationId);
    }
}