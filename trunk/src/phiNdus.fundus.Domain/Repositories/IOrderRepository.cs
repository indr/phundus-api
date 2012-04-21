using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        ICollection<Order> FindAll();
        ICollection<Order> FindPending();
        ICollection<Order> FindApproved();
        ICollection<Order> FindRejected();
        int SumReservedAmount(int articleId);
        Order FindCart(int userId);
    }
}