namespace Phundus.Core.Shop.Queries.Models
{
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using NHibernate.Criterion;

    public class OrderReadModelReader : ReadModelReaderBase, IOrderQueries
    {
        public OrderDto FindById(int orderId, int userId)
        {
            var ctx = CreateCtx();
            var op = new DataLoadOptions();
            op.LoadWith<OrderDto>(x => x.Items);
            ctx.LoadOptions = op;

            return (from o in ctx.OrderDtos
                where o.Id == orderId
                select o).FirstOrDefault();
        }

        public IEnumerable<OrderDto> FindByOrganizationId(int organizationId, int currentUserId)
        {
            return (from o in CreateCtx().OrderDtos
                where o.OrganizationId == organizationId
                select o);
        }

        public IEnumerable<OrderDto> FindByOrganizationId(int organizationId, int userId, OrderStatusDto status)
        {
            return (from o in CreateCtx().OrderDtos
                where o.OrganizationId == organizationId && o.Status == status
                select o);
        }

        public IEnumerable<OrderDto> FindByUserId(int userId)
        {
            return (from o in CreateCtx().OrderDtos
                where o.Borrower_Id == userId
                select o);
        }

        public OrderDto FindOrder(int orderId, int organizationId, int currentUserId)
        {
            var ctx = CreateCtx();
            var op = new DataLoadOptions();
            op.LoadWith<OrderDto>(x => x.Items);
            ctx.LoadOptions = op;

            return (from o in ctx.OrderDtos
                where o.Id == orderId
                select o).FirstOrDefault();
        }
    }
}