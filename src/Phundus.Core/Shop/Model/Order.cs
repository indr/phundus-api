namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Domain.Model;
    using Orders.Model;

    public class Order : EventSourcedAggregate
    {
        private OrderLines _orderLines = new OrderLines();

        public Order(Initiator initiator, OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee)
        {
            Apply(new OrderCreated(initiator, orderId, orderShortId, lessor, lessee));
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
        }

        public virtual void Reject(Initiator initiator)
        {
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();

            Apply(new OrderRejected(initiator, OrderId, OrderShortId, Lessor, Lessee, Status,
                OrderTotal, CreateOrderEventItems()));
        }

        protected void When(OrderRejected e)
        {
            Status = OrderStatus.Rejected;
        }


        public virtual void Approve(Initiator initiator)
        {
            AssertPending();

            Apply(new OrderApproved(initiator, OrderId, OrderShortId, Lessor, Lessee, Status,
                OrderTotal, CreateOrderEventItems()));
        }

        protected void When(OrderApproved e)
        {
            Status = OrderStatus.Approved;
        }


        public virtual void Close(Initiator initiator)
        {
            AssertNotRejected();
            AssertNotClosed();

            Apply(new OrderClosed(initiator, OrderId, OrderShortId, Lessor, Lessee, Status,
                OrderTotal, CreateOrderEventItems()));
        }

        protected void When(OrderClosed e)
        {
            Status = OrderStatus.Closed;
        }


        public virtual void AddItem(Initiator initiator, OrderLineId orderLineId, Article article, Period period,
            int quantity)
        {
            AssertPending();

            var unitPricePerWeek = article.Price;
            var priceInfo = new PerDayWithPerSevenDaysPricePricingStrategy()
                .Calculate(period, quantity, unitPricePerWeek);

            Apply(new OrderItemAdded(initiator, OrderId, OrderShortId, (int) Status, OrderTotal + priceInfo.Price,
                new OrderEventItem(orderLineId, article.ArticleId, article.ArticleShortId,
                    article.Name, unitPricePerWeek, period, quantity, priceInfo.Price)));
        }

        protected void When(OrderItemAdded e)
        {
            _orderLines.When(e);
        }

        public virtual void RemoveItem(Initiator initiator, Guid orderItemId)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemRemoved(initiator, OrderId, OrderShortId, Status, OrderTotal - item.LineTotal,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemRemoved e)
        {
            _orderLines.When(e);            
        }


        public virtual void ChangeAmount(Initiator initiator, Guid orderItemId, int amount)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemQuantityChanged(initiator, OrderId, OrderShortId,
                (int) Status, OrderTotal, item.LineId, item.Quantity, amount,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemQuantityChanged e)
        {
            _orderLines.When(e);
        }


        public virtual void ChangeItemPeriod(Initiator initiator, Guid orderItemId, DateTime fromUtc, DateTime toUtc)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemPeriodChanged(initiator, OrderId, OrderShortId, (int) Status, OrderTotal, orderItemId,
                item.Period, new Period(fromUtc, toUtc), CreateOrderEventItem(item)));
        }

        protected void When(OrderItemPeriodChanged e)
        {
            _orderLines.When(e);
        }


        public virtual void ChangeItemTotal(Initiator initiator, Guid orderItemId, decimal itemTotal)
        {
            AssertPending();

            var item = _orderLines.GetOrderLine(orderItemId);

            Apply(new OrderItemTotalChanged(initiator, OrderId, OrderShortId,
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

        private IList<OrderEventItem> CreateOrderEventItems()
        {
            return Lines.Select(CreateOrderEventItem).ToList();
        }

        private OrderEventItem CreateOrderEventItem(OrderLine line)
        {
            return new OrderEventItem(line.LineId, line.ArticleId, line.ArticleShortId, line.Text, line.UnitPricePerWeek,
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