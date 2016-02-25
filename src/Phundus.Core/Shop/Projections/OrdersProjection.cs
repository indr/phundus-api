namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Queries;

    public interface IOrderQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="shortOrderId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrderDto GetById(CurrentUserId currentUserId, ShortOrderId shortOrderId);

        IEnumerable<OrderDto> Query(CurrentUserId currentUserId, ShortOrderId shortOrderId, UserId queryUserId, OrganizationId queryOrganizationId);
    }

    public class OrdersProjection : ReadModelBase<OrdersProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }
    }

    public class OrdersProjectionRow
    {
        public virtual Guid OrderId { get; set; }
        public virtual int OrderShortId { get; set; }
        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual OrderStatus Status { get; set; }

        public virtual Guid LessorId { get; set; }
        public virtual string LessorName { get; set; }
        public virtual string LessorPostAddress { get; set; }
        public virtual string LessorEmailAddress { get; set; }
        public virtual string LessorPhoneNumber { get; set; }

        public virtual Guid LesseeId { get; set; }
        public virtual string LesseeName { get; set; }
        public virtual string LesseePostAddress { get; set; }
        public virtual string LesseeEmailAddress { get; set; }
        public virtual string LesseePhoneNumber { get; set; }

        public virtual ICollection<OrderItemsProjectionRow> Items { get; set; }

        public enum OrderStatus
        {
            Pending = 1,
            Approved,
            Rejected,
            Closed
        }
    }

    public class OrderItemsProjectionRow
    {
        public virtual Guid Id { get; set; }
        public virtual Guid OrderId { get; set; }
    }
}