namespace Phundus.Shop.Orders.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrderRepository : IRepository<Order>
    {
        Order GetById(int orderId);

        ICollection<Order> FindByUserId(Guid userId);
        
        IEnumerable<Order> FindByOrganizationId(Guid organizationId);
        IEnumerable<Order> FindByOrganizationId(Guid organizationId, OrderStatus status);

        int SumReservedAmount(int articleId);

        new int Add(Order entity);
    }
}