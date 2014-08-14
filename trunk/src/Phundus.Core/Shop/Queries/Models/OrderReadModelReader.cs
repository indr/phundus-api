namespace Phundus.Core.Shop.Queries.Models
{
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;

    public class OrderReadModelReader : ReadModelReaderBase, IOrderQueries
    {
        private static DataLoadOptions OrderDetailsDataLoadOptions
        {
            get
            {
                var op = new DataLoadOptions();
                op.LoadWith<OrderDto>(x => x.Items);
                op.LoadWith<OrderItemDto>(x => x.Article);
                return op;
            }
        }

        public OrderDto SingleByOrderId(int orderId, int currentUserId)
        {
            var ctx = CreateCtx();
            ctx.LoadOptions = OrderDetailsDataLoadOptions;

            return (from o in ctx.OrderDtos
                where (o.Id == orderId) && (o.Borrower_Id == currentUserId)
                select o).SingleOrDefault();
        }

        public OrderDto SingleByOrderIdAndOrganizationId(int orderId, int organizationId, int currentUserId)
        {
            var ctx = CreateCtx();
            ctx.LoadOptions = OrderDetailsDataLoadOptions;

            var query = from o in ctx.OrderDtos
                join m in ctx.MembershipDtos on
                    new {o.OrganizationId, Role = MembershipRoleDto.Chief, UserId = currentUserId}
                    equals new {m.OrganizationId, m.Role, m.UserId}
                where (o.Id == orderId && o.OrganizationId == organizationId)
                select o;

            return query.Distinct().SingleOrDefault();
        }

        public IEnumerable<OrderDto> ManyByUserId(int userId)
        {
            var ctx = CreateCtx();
            var query = from o in ctx.OrderDtos
                where o.Borrower_Id == userId
                select o;

            return query;
        }

        public IEnumerable<OrderDto> ManyByOrganizationId(int organizationId, int currentUserId)
        {
            var ctx = CreateCtx();
            return from o in ctx.OrderDtos
                        join m in ctx.MembershipDtos on
                            new { o.OrganizationId, Role = MembershipRoleDto.Chief, UserId = currentUserId }
                            equals new { m.OrganizationId, m.Role, m.UserId }
                        where (o.OrganizationId == organizationId)
                        select o;
        }
    }
}