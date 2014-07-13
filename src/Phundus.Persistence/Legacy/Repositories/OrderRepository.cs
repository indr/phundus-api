namespace Phundus.Persistence.Legacy.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using NHibernate.Linq;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
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

        public ICollection<Order> FindMy(int userId)
        {
            var query = from o in Orders
                        where o.Reserver.Id == userId
                        orderby o.Status ascending 
                        select o;
            return query.ToList();
        }

        public ICollection<Order> FindPending(Organization organization)
        {
            var query = from o in Orders
                        where o.Status == OrderStatus.Pending
                          && o.Organization.Id == organization.Id
                        select o;
            return query.ToList();
        }

        public ICollection<Order> FindApproved(Organization organization)
        {
            var query = from o in Orders
                        where o.Status == OrderStatus.Approved
                          && o.Organization.Id == organization.Id
                        select o;
            return query.ToList();
        }

        public ICollection<Order> FindRejected(Organization organization)
        {
            var query = from o in Orders
                        where o.Status == OrderStatus.Rejected
                          && o.Organization.Id == organization.Id
                        select o;
            return query.ToList();
        }

        public ICollection<Order> FindClosed(Organization organization)
        {
            var query = from o in Orders
                        where o.Status == OrderStatus.Closed
                          && o.Organization.Id == organization.Id
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

        #endregion
    }
}