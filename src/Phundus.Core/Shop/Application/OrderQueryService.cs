namespace Phundus.Shop.Application
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Common.Querying;
    using IdentityAccess.Application;
    using Inventory.Application;
    using NHibernate;
    using NHibernate.Criterion;

    public interface IOrderQueryService
    {
        OrderData GetById(InitiatorId initiatorId, OrderId orderId);

        IEnumerable<OrderData> Query(InitiatorId initiatorId, OrderId orderId, UserId queryUserId,
            OrganizationId queryOrganizationId);
    }

    public class OrderQueryService : QueryServiceBase<OrderData>, IOrderQueryService
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IMembershipQueries _membershipQueries;

        public OrderQueryService(IMembershipQueries membershipQueries, IAvailabilityService availabilityService)
        {
            if (membershipQueries == null) throw new ArgumentNullException("membershipQueries");
            if (availabilityService == null) throw new ArgumentNullException("availabilityService");

            _membershipQueries = membershipQueries;
            _availabilityService = availabilityService;
        }

        public OrderData GetById(InitiatorId initiatorId, OrderId orderId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");

            var result = Query(initiatorId, orderId == null ? (Guid?) null : orderId.Id, null, null)
                .SingleOrDefault();
            if (result == null)
                throw new NotFoundException(String.Format("Order {0} not found.", orderId));

            CalculateAvailabilities(result);

            return result;
        }

        public IEnumerable<OrderData> Query(InitiatorId initiatorId, OrderId orderId, UserId queryUserId,
            OrganizationId queryOrganizationId)
        {
            return Query(initiatorId, orderId == null ? (Guid?) null : orderId.Id,
                queryUserId == null ? (Guid?) null : queryUserId.Id,
                queryOrganizationId == null ? (Guid?) null : queryOrganizationId.Id);
        }

        private IEnumerable<OrderData> Query(InitiatorId initiatorId, Guid? queryOrderId, Guid? queryUserId,
            Guid? queryOrganizationId)
        {
            var query = QueryOver();

            AddAuthFilter(query, initiatorId);
            AddQueryFilter(query, queryOrderId, queryUserId, queryOrganizationId);

            return query.List();
        }

        private void AddAuthFilter(IQueryOver<OrderData, OrderData> query, InitiatorId initiatorId)
        {
            var organizationIds = _membershipQueries.FindByUserId(initiatorId.Id)
                .Where(p => p.MembershipRole == "Manager")
                .Select(s => s.OrganizationGuid).ToList();

            var lesseeOrLessor = new Disjunction();
            lesseeOrLessor.Add(Restrictions.Where<OrderData>(p => p.LesseeId == initiatorId.Id));
            lesseeOrLessor.Add(Restrictions.Where<OrderData>(p => p.LessorId == initiatorId.Id));

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

            foreach (var each in orderDto.Lines)
            {
                each.IsAvailable = _availabilityService.IsArticleAvailable(new ArticleId(each.ArticleId), each.FromUtc,
                    each.ToUtc,
                    each.Quantity, new OrderLineId(each.LineId));
            }
        }
    }

    public class OrderData
    {
        public enum OrderStatus
        {
            Pending = 1,
            Approved,
            Rejected,
            Closed
        }

        private ICollection<OrderLineData> _lines = new Collection<OrderLineData>();

        public virtual Guid OrderId { get; set; }
        public virtual int OrderShortId { get; set; }        
        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual DateTime ModifiedAtUtc { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual decimal OrderTotal { get; set; }

        public virtual Guid LessorId { get; set; }
        public virtual string LessorName { get; set; }
        public virtual string LessorStreet { get; set; }
        public virtual string LessorPostcode { get; set; }
        public virtual string LessorCity { get; set; }
        public virtual string LessorEmailAddress { get; set; }
        public virtual string LessorPhoneNumber { get; set; }

        public virtual Guid LesseeId { get; set; }
        public virtual string LesseeFirstName { get; set; }
        public virtual string LesseeLastName { get; set; }
        public virtual string LesseeStreet { get; set; }
        public virtual string LesseePostcode { get; set; }
        public virtual string LesseeCity { get; set; }
        public virtual string LesseeEmailAddress { get; set; }
        public virtual string LesseePhoneNumber { get; set; }

        public virtual ICollection<OrderLineData> Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }
    }

    public class OrderLineData
    {
        public virtual Guid DataId { get; set; }

        public virtual Guid LineId { get; set; }
        public virtual OrderData Order { get; set; }

        public virtual Guid ArticleId { get; set; }
        public virtual int ArticleShortId { get; set; }

        public virtual string Text { get; set; }
        public virtual DateTime FromUtc { get; set; }
        public virtual DateTime ToUtc { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPricePerWeek { get; set; }
        public virtual decimal LineTotal { get; set; }
        public virtual bool IsAvailable { get; set; }
    }
}