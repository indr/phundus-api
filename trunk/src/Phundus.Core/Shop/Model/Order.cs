namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Orders.Model;

    public class Order : EventSourcedAggregateRoot
    {
        private OrderLines _orderLines;

        public Order(Manager manager, OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee)
            : this(new Initiator(new InitiatorId(manager.UserId.Id), manager.EmailAddress, manager.FullName),
                orderId, orderShortId, lessor, lessee)
        {
        }

        public Order(Initiator initiator, OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee,
            OrderLines orderLines = null)
        {
            Apply(new OrderCreated(initiator, orderId, orderShortId, lessor, lessee, OrderStatus.Pending,
                orderLines == null ? 0.0m : orderLines.GetOrderLinesSum(), CreateOrderEventItems(orderLines)));
        }

        protected Order()
        {
        }

        protected Order(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        public virtual OrderId OrderId { get; private set; }
        public virtual OrderShortId OrderShortId { get; private set; }
        public virtual DateTime CreatedAtUtc { get; private set; }
        public virtual OrderStatus Status { get; private set; }
        public virtual Lessor Lessor { get; private set; }
        public virtual Lessee Lessee { get; private set; }

        public virtual decimal OrderTotal
        {
            get { return _orderLines.GetOrderLinesSum(); }
        }

        public virtual ICollection<OrderLine> Lines
        {
            get { return _orderLines.Lines; }
        }

        protected void When(OrderCreated e)
        {
            OrderId = new OrderId(e.OrderId);
            OrderShortId = new OrderShortId(e.OrderShortId);
            CreatedAtUtc = e.OccuredOnUtc;
            Status = OrderStatus.Pending;
            Lessee = e.Lessee;
            Lessor = e.Lessor;
            _orderLines = new OrderLines(e.Lines);
        }

        public virtual void Reject(Manager manager)
        {
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();

            Apply(new OrderRejected(manager, OrderId, OrderShortId, Lessor, Lessee, Status,
                OrderTotal, CreateOrderEventItems(_orderLines)));
        }

        protected void When(OrderRejected e)
        {
            Status = OrderStatus.Rejected;
        }


        public virtual void Approve(Manager manager)
        {
            AssertPending();

            Apply(new OrderApproved(manager, OrderId, OrderShortId, Lessor, Lessee, Status,
                OrderTotal, CreateOrderEventItems(_orderLines)));
        }

        protected void When(OrderApproved e)
        {
            Status = OrderStatus.Approved;
        }


        public virtual void Close(Manager manager)
        {
            AssertNotRejected();
            AssertNotClosed();

            Apply(new OrderClosed(manager, OrderId, OrderShortId, Lessor, Lessee, Status,
                OrderTotal, CreateOrderEventItems(_orderLines)));
        }

        protected void When(OrderClosed e)
        {
            Status = OrderStatus.Closed;
        }


        public virtual void AddItem(Manager manager, OrderLineId orderLineId, Article article, Period period,
            int quantity, decimal lineTotal)
        {
            AssertPending();

            var unitPricePerWeek = article.Price;            

            Apply(new OrderItemAdded(manager, OrderId, OrderShortId, (int) Status, OrderTotal + lineTotal,
                new OrderEventLine(orderLineId, article.ArticleId, article.ArticleShortId,
                    article.Name, unitPricePerWeek, period, quantity, lineTotal)));
        }

        protected void When(OrderItemAdded e)
        {
            _orderLines.When(e);
        }

        public virtual void RemoveItem(Manager manager, Guid orderItemId)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemRemoved(manager, OrderId, OrderShortId, Status, OrderTotal - item.LineTotal,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemRemoved e)
        {
            _orderLines.When(e);
        }


        public virtual void ChangeQuantity(Manager manager, Guid orderItemId, int quantity)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemQuantityChanged(manager, OrderId, OrderShortId,
                (int) Status, OrderTotal, item.LineId, item.Quantity, quantity,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemQuantityChanged e)
        {
            _orderLines.When(e);
        }


        public virtual void ChangeItemPeriod(Manager manager, Guid orderItemId, DateTime fromUtc, DateTime toUtc)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemPeriodChanged(manager, OrderId, OrderShortId, (int) Status, OrderTotal, orderItemId,
                item.Period, new Period(fromUtc, toUtc), CreateOrderEventItem(item)));
        }

        protected void When(OrderItemPeriodChanged e)
        {
            _orderLines.When(e);
        }


        public virtual void ChangeItemTotal(Manager manager, Guid orderItemId, decimal itemTotal)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemTotalChanged(manager, OrderId, OrderShortId,
                (int) Status, OrderTotal, item.LineId, item.LineTotal, itemTotal,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemTotalChanged e)
        {
            _orderLines.When(e);
        }

        private void AssertPending()
        {
            switch (Status)
            {
                case OrderStatus.Pending:
                    break;
                case OrderStatus.Approved:
                    throw new OrderAlreadyApprovedException();
                case OrderStatus.Rejected:
                    throw new OrderAlreadyRejectedException();
                case OrderStatus.Closed:
                    throw new OrderAlreadyClosedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AssertNotRejected()
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
        }

        private void AssertNotClosed()
        {
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
        }

        private static IList<OrderEventLine> CreateOrderEventItems(OrderLines orderLines)
        {
            if (orderLines == null)
                return null;
            return orderLines.Lines.Select(CreateOrderEventItem).ToList();
        }

        private static OrderEventLine CreateOrderEventItem(OrderLine line)
        {
            return new OrderEventLine(line.LineId, line.ArticleId, line.ArticleShortId, line.Text, line.UnitPricePerWeek,
                line.Period, line.Quantity, line.LineTotal);
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return OrderId;
        }
    }

    public class OrderAlreadyClosedException : Exception
    {
    }

    public class OrderAlreadyRejectedException : Exception
    {
    }

    public class OrderAlreadyApprovedException : Exception
    {
    }
}