namespace Phundus.Shop.Queries.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Inventory.Services;
    using Projections;

    public class OrderReadModelReader : ReadModelReaderBase, IOrderQueries
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IMembershipQueries _membershipQueries;

        public OrderReadModelReader(IMembershipQueries membershipQueries,
            IUsersQueries usersQueries,
            IAvailabilityService availabilityService)
        {
            AssertionConcern.AssertArgumentNotNull(membershipQueries, "MembershipQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(usersQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(availabilityService, "AvailabilityService must be provided.");

            _membershipQueries = membershipQueries;
            _availabilityService = availabilityService;
        }

        private static DataLoadOptions OrderDetailsDataLoadOptions
        {
            get
            {
                var op = new DataLoadOptions();
                op.LoadWith<OrderDto>(x => x.Items);
                return op;
            }
        }

        public OrderDto GetById(CurrentUserId currentUserId, ShortOrderId shortOrderId)
        {
            AssertionConcern.AssertArgumentNotNull(currentUserId, "CurrentUserId must be provided.");
            AssertionConcern.AssertArgumentNotNull(shortOrderId, "OrderId must be provided.");

            var result =
                Query(currentUserId, shortOrderId == null ? (int?) null : shortOrderId.Id, null, null).SingleOrDefault();
            if (result == null)
                throw new NotFoundException(String.Format("Order {0} not found.", shortOrderId));

            CalculateAvailabilities(result);

            return result;
        }

        public IEnumerable<OrderDto> Query(CurrentUserId currentUserId, ShortOrderId shortOrderId, UserId queryUserId,
            OrganizationId queryOrganizationId)
        {
            return Query(currentUserId, shortOrderId == null ? (int?) null : shortOrderId.Id,
                queryUserId == null ? (Guid?) null : queryUserId.Id,
                queryOrganizationId == null ? (Guid?) null : queryOrganizationId.Id);
        }

        private IEnumerable<OrderDto> Query(CurrentUserId currentUserId, int? orderId, Guid? queryUserGuid,
            Guid? queryOrganizationId)
        {
            AssertionConcern.AssertArgumentNotNull(currentUserId, "CurrentUserId must be provided.");

            var currentUsersManagerGuids =
                _membershipQueries.FindByUserId(currentUserId.Id)
                    .Where(p => p.MembershipRole == "Manager")
                    .Select(s => s.OrganizationGuid);
            AssertionConcern.AssertArgumentNotNull(currentUsersManagerGuids,
                "CurrentUsersManagerGuids must be provided.");

            var ctx = CreateCtx();
            //ctx.LoadOptions = OrderDetailsDataLoadOptions;
            var q = from o in ctx.OrderDtos
                where
                    (
                        // Auth restrictions
                        (
                            // current user is borrower or lessor
                            (o.Lessee_LesseeGuid == currentUserId.Id || o.Lessor_LessorId == currentUserId.Id)
                            ||
                            // Or current user is manager in lessor organization
                            (currentUsersManagerGuids.Contains(o.Lessor_LessorId))
                            )
                        &&
                        // Query restrictions
                        (
                            // orderId is not queried, or order id is query order id
                            ((orderId == null) || (o.Id == orderId))
                            &&
                            // and user is not queried, or borrower or lessor is query user
                            (queryUserGuid == null
                             || (o.Lessee_LesseeGuid == queryUserGuid || o.Lessor_LessorId == queryUserGuid))
                            &&
                            // and organization is not queried, or lessor is query organization
                            (queryOrganizationId == null || (o.Lessor_LessorId == queryOrganizationId))
                            )
                        )
                select o;

            return q;
        }

        private void CalculateAvailabilities(OrderDto orderDto)
        {
            if (orderDto == null)
                return;

            foreach (var each in orderDto.Items)
            {
                each.IsAvailable = _availabilityService.IsArticleAvailable(each.ArticleId, each.FromUtc, each.ToUtc,
                    each.Amount, each.Id);
            }
        }
    }
}