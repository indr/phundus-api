using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        ICollection<Order> FindMy(int userId);
        ICollection<Order> FindPending();
        ICollection<Order> FindApproved();
        ICollection<Order> FindRejected();
        int SumReservedAmount(int articleId);
        ICollection<Order> FindClosed();
    }
}