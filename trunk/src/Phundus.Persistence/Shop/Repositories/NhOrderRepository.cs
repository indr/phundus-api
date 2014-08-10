namespace Phundus.Persistence.Shop.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;
    using NHibernate.Linq;

    public class NhOrderRepository : NhRepositoryBase<Order>, IOrderRepository
    {
        private IQueryable<OrderItem> Items
        {
            get { return Session.Query<OrderItem>(); }
        }

        public ICollection<Order> FindMy(int userId)
        {
            var query = from o in Entities
                where o.Reserver.Id == userId
                orderby o.Status ascending
                select o;
            return query.ToList();
        }

        public ICollection<Order> FindPending(int organizationId)
        {
            var query = from o in Entities
                where o.Status == OrderStatus.Pending
                      && o.Organization.Id == organizationId
                select o;
            return query.ToList();
        }

        public ICollection<Order> FindApproved(int organizationId)
        {
            var query = from o in Entities
                where o.Status == OrderStatus.Approved
                      && o.Organization.Id == organizationId
                select o;
            return query.ToList();
        }

        public ICollection<Order> FindRejected(int organizationId)
        {
            var query = from o in Entities
                where o.Status == OrderStatus.Rejected
                      && o.Organization.Id == organizationId
                select o;
            return query.ToList();
        }

        public ICollection<Order> FindClosed(int organizationId)
        {
            var query = from o in Entities
                where o.Status == OrderStatus.Closed
                      && o.Organization.Id == organizationId
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

        public IEnumerable<Order> Find(int organizationId, OrderStatus status)
        {
            return (from o in Entities
                where o.Status == status && o.Organization.Id == organizationId
                select o).ToFuture();
        }
    }
}