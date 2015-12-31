namespace Phundus.Core.Shop.Queries.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using IdentityAndAccess.Queries;
    using Inventory.Services;

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

        public IAvailabilityService AvailabilityService { get; set; }

        public IUserQueries UserQueries { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        public OrderDto SingleByOrderId(int orderId, int currentUserId)
        {
            var ctx = CreateCtx();
            ctx.LoadOptions = OrderDetailsDataLoadOptions;

            var result = (from o in ctx.OrderDtos
                where (o.Id == orderId) && (o.Borrower_Id == currentUserId)
                select o).SingleOrDefault();

            CalculateAvailabilities(result);

            return result;
        }

        public OrderDto SingleByOrderIdAndOrganizationId(int orderId, Guid organizationId, int currentUserId)
        {
            var ctx = CreateCtx();
            ctx.LoadOptions = OrderDetailsDataLoadOptions;

            var query = from o in ctx.OrderDtos
                join m in ctx.MembershipDtos on
                    new {Role = MembershipRoleDto.Chief, UserId = currentUserId}
                    equals new {m.Role, m.UserId}
                where (o.Id == orderId && o.Lessor_LessorId == organizationId && m.OrganizationGuid == organizationId)
                select o;

            var result = query.Distinct().SingleOrDefault();

            CalculateAvailabilities(result);

            return result;
        }

        public IEnumerable<OrderDto> ManyByUserId(int userId)
        {
            var ctx = CreateCtx();
            var query = from o in ctx.OrderDtos
                where o.Borrower_Id == userId
                select o;

            return query;
        }

        public IEnumerable<OrderDto> ManyByOrganizationId(Guid organizationId, int currentUserId)
        {
            var ctx = CreateCtx();
            return from o in ctx.OrderDtos
                join m in ctx.MembershipDtos on
                    new {Role = MembershipRoleDto.Chief, UserId = currentUserId}
                    equals new {m.Role, m.UserId}
                where (o.Lessor_LessorId == organizationId && m.OrganizationGuid == organizationId)
                select o;
        }

        public IEnumerable<OrderDto> Query(int currentUserId, int? orderId, int? queryUserId, Guid? queryOrganizationId)
        {
            var currentUserGuid = UserQueries.GetById(currentUserId).Guid;
            var queryUserGuid = (Guid?) null;
            if (queryUserId.HasValue)
            {
                queryUserGuid = UserQueries.GetById(queryUserId.Value).Guid;
            }

            var currentUsersManagerGuids =
                MembershipQueries.ByUserId(currentUserId)
                    .Where(p => p.MembershipRole == "Chief")
                    .Select(s => s.OrganizationGuid);

            var ctx = CreateCtx();
            var q = from o in ctx.OrderDtos
                where (
                    // Auth restrictions
                    (
                        // current user is borrower or lessor
                        ((o.Borrower_Id == currentUserId || o.Lessor_LessorId == currentUserGuid))
                        ||
                        // Or: current user is manager in lessor organization
                        (currentUsersManagerGuids.Contains(o.Lessor_LessorId))
                        )
                    &&
                    // Query restrictions
                    (
                        // orderId is not queried, or order id is query order id
                        (orderId == null || (o.Id == orderId))
                        &&
                        // user is not queried, or borrower or lessor is query user
                        (queryUserId == null
                         || (o.Borrower_Id == queryUserId || o.Lessor_LessorId == queryUserGuid))
                        &&
                        // organization is not queried, or lessor is query organization
                        (queryOrganizationId == null || (o.Lessor_LessorId == queryOrganizationId))
                        ))
                select o;

            return q;
        }

        private void CalculateAvailabilities(OrderDto orderDto)
        {
            if (orderDto == null)
                return;

            foreach (var each in orderDto.Items)
            {
                each.IsAvailable = AvailabilityService.IsArticleAvailable(each.ArticleId, each.FromUtc, each.ToUtc,
                    each.Amount, each.Id);
            }
        }
    }
}