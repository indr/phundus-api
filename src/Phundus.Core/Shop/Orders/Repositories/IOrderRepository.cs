namespace Phundus.Core.Shop.Orders.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrderRepository : IRepository<Order>
    {
        ICollection<Order> FindMy(int userId);
        ICollection<Order> FindPending(int organizationId);
        ICollection<Order> FindApproved(int organizationId);
        ICollection<Order> FindRejected(int organizationId);
        ICollection<Order> FindClosed(int organizationId);
        int SumReservedAmount(int articleId);
    }
}