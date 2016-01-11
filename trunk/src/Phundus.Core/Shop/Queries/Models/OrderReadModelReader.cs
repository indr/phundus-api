namespace Phundus.Core.Shop.Queries.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Queries;
    using Inventory.Services;
    using Phundus.Shop.Queries;

    public class OrderReadModelReader : ReadModelReaderBase, IOrderQueries
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IMembershipQueries _membershipQueries;
        private readonly IUserQueries _userQueries;

        public OrderReadModelReader(IMembershipQueries membershipQueries, IUserQueries userQueries,
            IAvailabilityService availabilityService)
        {
            AssertionConcern.AssertArgumentNotNull(membershipQueries, "MembershipQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(availabilityService, "AvailabilityService must be provided.");

            _membershipQueries = membershipQueries;
            _userQueries = userQueries;
            _availabilityService = availabilityService;
        }

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

        public OrderDto GetById(CurrentUserId currentUserId, OrderId orderId)
        {
            AssertionConcern.AssertArgumentNotNull(currentUserId, "CurrentUserId must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "OrderId must be provided.");

            var result = Query(currentUserId, orderId == null ? (int?) null : orderId.Id, null, null).SingleOrDefault();
            if (result == null)
                throw new NotFoundException(String.Format("Order {0} not found.", orderId));

            CalculateAvailabilities(result);

            return result;
        }

        public IEnumerable<OrderDto> Query(CurrentUserId currentUserId, OrderId orderId, int? queryUserId,
            Guid? queryOrganizationId)
        {
            return Query(currentUserId, orderId == null ? (int?) null : orderId.Id, queryUserId, queryOrganizationId);
        }

        private IEnumerable<OrderDto> Query(UserId currentUserId, int? orderId, int? queryUserId,
            Guid? queryOrganizationId)
        {
            AssertionConcern.AssertArgumentNotNull(currentUserId, "CurrentUserId must be provided.");

            var currentUserGuid = _userQueries.GetById(currentUserId).Guid;
            var queryUserGuid = (Guid?) null;
            if (queryUserId.HasValue)
            {
                queryUserGuid = _userQueries.GetById(queryUserId.Value).Guid;
            }

            var currentUsersManagerGuids =
                _membershipQueries.ByUserId(currentUserId.Id)
                    .Where(p => p.MembershipRole == "Chief")
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
                            (o.Borrower_Id == currentUserId.Id || o.Lessor_LessorId == currentUserGuid)
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
                            (queryUserId == null
                             || (o.Borrower_Id == queryUserId || o.Lessor_LessorId == queryUserGuid))
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