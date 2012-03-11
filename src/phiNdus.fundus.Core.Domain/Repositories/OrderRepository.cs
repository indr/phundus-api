using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class OrderRepository : NHRepository<Order>, IOrderRepository
    {
        private IQueryable<Order> Orders
        {
            get { return Session.Query<Order>(); }
        }

        private IQueryable<OrderItem> Items
        {
            get { return Session.Query<OrderItem>(); }
        }

        #region IOrderRepository Members

        public ICollection<Order> FindAll()
        {
            var query = from o in Orders
                        select o;
            return query.ToList();
        }

        public ICollection<Order> FindPending()
        {
            var query = from o in Orders
                        where o.Status == OrderStatus.Pending
                        select o;
            return query.ToList();
        }

        public ICollection<Order> FindApproved()
        {
            var query = from o in Orders
                        where o.Status == OrderStatus.Approved
                        select o;
            return query.ToList();
        }

        public ICollection<Order> FindRejected()
        {
            var query = from o in Orders
                        where o.Status == OrderStatus.Rejected
                        select o;
            return query.ToList();
        }

        public int SumReservedAmount(int articleId)
        {
            var query = from i in Items
                        where i.Article.Id == articleId
                              && (i.Order.Status == OrderStatus.Pending || i.Order.Status == OrderStatus.Approved) 
                        select i;
            return query.Sum(x => (int?) x.Amount).GetValueOrDefault();
        }

        public Order FindCart(int userId)
        {
            var q = (from o in Orders
                     where o.Status == OrderStatus.Card
                           && o.Reserver.Id == userId
                     select o);
            return q.FirstOrDefault();
        }

        #endregion
    }
}