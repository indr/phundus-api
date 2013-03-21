using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        ICollection<Order> FindMy(int userId);
        ICollection<Order> FindPending(Organization organization);
        ICollection<Order> FindApproved(Organization organization);
        ICollection<Order> FindRejected(Organization organization);
        ICollection<Order> FindClosed(Organization organization);
        int SumReservedAmount(int articleId);
        
    }
}