namespace Phundus.Persistence.Shop.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;
    using NHibernate.Linq;

    public class NhOrderRepository : NhRepositoryBase<Order>, IOrderRepository
    {
        private IQueryable<OrderItem> Items
        {
            get { return Session.Query<OrderItem>(); }
        }

        public Order GetById(int id)
        {
            var result = ById(id);
            if (result == null)
                throw new OrderNotFoundException(id);
            return result;
        }

        public ICollection<Order> FindByUserId(int userId)
        {
            var query = from o in Entities
                where o.Reserver.Id == userId
                orderby o.Status ascending
                select o;
            return query.ToList();
        }

        public IEnumerable<Order> FindByOrganizationId(int organizationId)
        {
            return (from o in Entities
                    where o.OrganizationId == organizationId
                    select o).ToFuture();
        }

        public IEnumerable<Order> FindByOrganizationId(int organizationId, OrderStatus status)
        {
            return (from o in Entities
                    where o.Status == status && o.OrganizationId == organizationId
                    select o).ToFuture();
        }

        public int SumReservedAmount(int articleId)
        {
            var query = from i in Items
                where i.Article.Id == articleId
                      && (i.Order.Status == OrderStatus.Pending || i.Order.Status == OrderStatus.Approved)
                select i;
            return query.Sum(x => (int?) x.Amount).GetValueOrDefault();
        }

        public new int Add(Order entity)
        {
            base.Add(entity);
            return entity.Id;
        }
    }
}