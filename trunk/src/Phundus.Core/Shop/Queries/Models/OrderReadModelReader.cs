namespace Phundus.Shop.Queries.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using IdentityAccess.Users.Model;
    using Integration.IdentityAccess;
    using Inventory.Services;

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

        public OrderDto GetById(CurrentUserGuid currentUserGuid, OrderId orderId)
        {
            AssertionConcern.AssertArgumentNotNull(currentUserGuid, "CurrentUserId must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "OrderId must be provided.");

            var result = Query(currentUserGuid, orderId == null ? (int?) null : orderId.Id, null, null).SingleOrDefault();
            if (result == null)
                throw new NotFoundException(String.Format("Order {0} not found.", orderId));

            CalculateAvailabilities(result);

            return result;
        }

        public IEnumerable<OrderDto> Query(CurrentUserGuid currentUserGuid, OrderId orderId, UserGuid queryUserGuid,
            OrganizationGuid queryOrganizationId)
        {
            return Query(currentUserGuid, orderId == null ? (int?) null : orderId.Id,
                queryUserGuid == null ? (Guid?) null : queryUserGuid.Id,
                queryOrganizationId == null ? (Guid?) null : queryOrganizationId.Id);
        }

        private IEnumerable<OrderDto> Query(CurrentUserGuid currentUserGuid, int? orderId, Guid? queryUserGuid,
            Guid? queryOrganizationId)
        {
            AssertionConcern.AssertArgumentNotNull(currentUserGuid, "CurrentUserId must be provided.");
            
            var currentUsersManagerGuids =
                _membershipQueries.ByUserId(currentUserGuid.Id)
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
                            (o.Lessee_LesseeGuid == currentUserGuid.Id || o.Lessor_LessorId == currentUserGuid.Id)
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