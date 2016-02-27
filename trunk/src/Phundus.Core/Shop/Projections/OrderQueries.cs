namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Projections;
    using Inventory.Application;
    using NHibernate;
    using NHibernate.Criterion;

    public interface IOrderQueries
    {
        OrderData GetById(CurrentUserId currentUserId, OrderId orderId);

        IEnumerable<OrderData> Query(CurrentUserId currentUserId, OrderId orderId, UserId queryUserId,
            OrganizationId queryOrganizationId);
    }

    public class OrderQueries : QueryBase<OrderData>, IOrderQueries
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IMembershipQueries _membershipQueries;

        public OrderQueries(IMembershipQueries membershipQueries, IAvailabilityService availabilityService)
        {
            if (membershipQueries == null) throw new ArgumentNullException("membershipQueries");
            if (availabilityService == null) throw new ArgumentNullException("availabilityService");

            _membershipQueries = membershipQueries;
            _availabilityService = availabilityService;
        }

        public OrderData GetById(CurrentUserId currentUserId, OrderId orderId)
        {
            if (currentUserId == null) throw new ArgumentNullException("currentUserId");

            var result =
                Query(currentUserId, orderId == null ? (Guid?) null : orderId.Id, null, null).SingleOrDefault();
            if (result == null)
                throw new NotFoundException(String.Format("Order {0} not found.", orderId));

            CalculateAvailabilities(result);

            return result;
        }

        public IEnumerable<OrderData> Query(CurrentUserId currentUserId, OrderId orderId, UserId queryUserId,
            OrganizationId queryOrganizationId)
        {
            return Query(currentUserId, orderId == null ? (Guid?) null : orderId.Id,
                queryUserId == null ? (Guid?) null : queryUserId.Id,
                queryOrganizationId == null ? (Guid?) null : queryOrganizationId.Id);
        }

        private IEnumerable<OrderData> Query(CurrentUserId currentUserId, Guid? queryOrderId, Guid? queryUserId,
            Guid? queryOrganizationId)
        {
            var query = QueryOver();

            AddAuthFilter(query, currentUserId);
            AddQueryFilter(query, queryOrderId, queryUserId, queryOrganizationId);

            return query.List();
        }

        private void AddAuthFilter(IQueryOver<OrderData, OrderData> query, CurrentUserId currentUserId)
        {
            var organizationIds = _membershipQueries.FindByUserId(currentUserId.Id)
                .Where(p => p.MembershipRole == "Manager")
                .Select(s => s.OrganizationGuid).ToList();

            var lesseeOrLessor = new Disjunction();
            lesseeOrLessor.Add(Restrictions.Where<OrderData>(p => p.LesseeId == currentUserId.Id));
            lesseeOrLessor.Add(Restrictions.Where<OrderData>(p => p.LessorId == currentUserId.Id));

            var authRestriction = new Disjunction();
            authRestriction.Add(lesseeOrLessor);
            authRestriction.Add(Restrictions.On<OrderData>(p => p.LessorId).IsIn(organizationIds));

            query.Where(authRestriction);
        }

        private void AddQueryFilter(IQueryOver<OrderData, OrderData> query, Guid? queryOrderId, Guid? queryUserId,
          Guid? queryOrganizationId)
        {
            if (queryOrderId.HasValue)
            {
                query.And(p => p.OrderId == queryOrderId.Value);
            }
            if (queryUserId.HasValue)
            {
                query.And(p => p.LessorId == queryUserId.Value || p.LesseeId == queryUserId.Value);
            }
            if (queryOrganizationId.HasValue)
            {
                query.And(p => p.LessorId == queryOrganizationId.Value);
            }
        }

        private void CalculateAvailabilities(OrderData orderDto)
        {
            if (orderDto == null)
                return;

            foreach (var each in orderDto.Items)
            {
                each.IsAvailable = _availabilityService.IsArticleAvailable(each.ArticleId, each.FromUtc, each.ToUtc,
                    each.Quantity, each.Id);
            }
        }
    }
}