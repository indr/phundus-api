namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Iesi.Collections.Generic;
    using Orders.Model;

    public class Order : EventSourcedAggregate
    {
        private Iesi.Collections.Generic.ISet<OrderLine> _items = new HashedSet<OrderLine>();

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

        public virtual Iesi.Collections.Generic.ISet<OrderLine> Items
        {
            get { return new ImmutableSet<OrderLine>(_items); }
            protected set { _items = value; }
        }

        public virtual decimal OrderTotal
        {
            get { return _items.Sum(x => x.LineTotal); }
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
            var item = new OrderLine(new OrderLineId(e.OrderItem.ItemId), new ArticleId(e.OrderItem.ArticleId),
                new ArticleShortId(e.OrderItem.ArticleShortId),
                e.OrderItem.Text, e.OrderItem.Period, e.OrderItem.Quantity, e.OrderItem.UnitPricePerWeek,
                e.OrderItem.ItemTotal);
            _items.Add(item);
        }

        public virtual void RemoveItem(Initiator initiator, Guid orderItemId)
        {
            AssertPending();

            var item = GetOrderLine(orderItemId);

            Apply(new OrderItemRemoved(initiator, OrderId, OrderShortId, Status, OrderTotal - item.LineTotal,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemRemoved e)
        {
            var item = GetOrderLine(e.OrderItem.ItemId);
            _items.Remove(item);
        }


        public virtual void ChangeAmount(Initiator initiator, Guid orderItemId, int amount)
        {
            AssertPending();

            var item = GetOrderLine(orderItemId);

            Apply(new OrderItemQuantityChanged(initiator, OrderId, OrderShortId,
                (int) Status, OrderTotal, item.LineId, item.Quantity, amount,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemQuantityChanged e)
        {
            var item = GetOrderLine(e.OrderItemId);
            item.ChangeQuantity(e.NewQuantity);
        }


        public virtual void ChangeItemPeriod(Initiator initiator, Guid orderItemId, DateTime fromUtc, DateTime toUtc)
        {
            AssertPending();

            var item = GetOrderLine(orderItemId);

            Apply(new OrderItemPeriodChanged(initiator, OrderId, OrderShortId, (int) Status, OrderTotal, orderItemId,
                item.Period, new Period(fromUtc, toUtc), CreateOrderEventItem(item)));
        }

        protected void When(OrderItemPeriodChanged e)
        {
            var item = GetOrderLine(e.OrderItemId);
            item.ChangePeriod(e.NewPeriod);
        }


        public virtual void ChangeItemTotal(Initiator initiator, Guid orderItemId, decimal itemTotal)
        {
            AssertPending();

            var item = GetOrderLine(orderItemId);

            Apply(new OrderItemTotalChanged(initiator, OrderId, OrderShortId,
                (int) Status, OrderTotal, item.LineId, item.LineTotal, itemTotal,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemTotalChanged e)
        {
            var item = GetOrderLine(e.OrderItemId);
            item.ChangeLineTotal(e.NewItemTotal);
        }


        private OrderLine GetOrderLine(Guid orderItemId)
        {
            var item = _items.FirstOrDefault(p => p.LineId.Id == orderItemId);
            if (item == null)
                throw new InvalidOperationException(String.Format("Could not find order line {0}", orderItemId));
            return item;
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
            return Items.Select(CreateOrderEventItem).ToList();
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