namespace Phundus.Persistence.Shop.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using NHibernate.Linq;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;

    public class NhOrderRepository : NhRepositoryBase<Order>, IOrderRepository
    {
        private IQueryable<OrderItem> Items
        {
            get { return Session.Query<OrderItem>(); }
        }

        public Order GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
                throw new NotFoundException("Order {0} not found.", id);
            return result;
        }

        public ICollection<Order> FindByUserId(Guid userId)
        {
            var query = from o in Entities
                where o.Lessee.LesseeId.Id == userId
                orderby o.Status ascending
                select o;
            return query.ToList();
        }

        public IEnumerable<Order> FindByOrganizationId(Guid organizationId)
        {
            return (from o in Entities
                where o.Lessor.LessorId.Id == organizationId
                select o).ToFuture();
        }

        public IEnumerable<Order> FindByOrganizationId(Guid organizationId, OrderStatus status)
        {
            return (from o in Entities
                where o.Status == status && o.Lessor.LessorId.Id == organizationId
                select o).ToFuture();
        }

        public int SumReservedAmount(int articleId)
        {
            var query = from i in Items
                where i.ArticleShortId.Id == articleId
                      && (i.Order.Status == OrderStatus.Pending || i.Order.Status == OrderStatus.Approved)
                select i;
            return query.Sum(x => (int?) x.Amount).GetValueOrDefault();
        }

        public new int Add(Order entity)
        {
            base.Add(entity);
            return entity.ShortOrderId.Id;
        }
    }
}