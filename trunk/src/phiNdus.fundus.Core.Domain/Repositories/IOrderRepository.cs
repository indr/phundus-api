using System.Collections.Generic;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        ICollection<Order> FindAll();
        ICollection<Order> FindPending();
        ICollection<Order> FindApproved();
        ICollection<Order> FindRejected();
        int CountReserved(int articleId);
        int SumReservedAmount(int articleId);
    }
}