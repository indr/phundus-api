namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Shop.Model;

    public class Order : EventSourcedAggregate
    {
        private DateTime _createdAtUtc = DateTime.UtcNow;
        private Iesi.Collections.Generic.ISet<OrderItem> _items = new HashedSet<OrderItem>();
        private Lessee _lessee;
        private Lessor _lessor;
        private OrderId _orderId;
        private OrderShortId _orderShortId;
        private OrderStatus _status = OrderStatus.Pending;

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

        public virtual int Id
        {
            get { return _orderShortId.Id; }
            protected set { _orderShortId = new OrderShortId(value); }
        }

        public virtual OrderShortId OrderShortId
        {
            get { return _orderShortId; }
            protected set { _orderShortId = value; }
        }

        public virtual OrderId OrderId
        {
            get { return _orderId; }
            protected set { _orderId = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual DateTime CreatedAtUtc
        {
            get { return _createdAtUtc; }
            set { _createdAtUtc = value; }
        }

        public virtual DateTime CreatedAtLocal
        {
            get { return CreatedAtUtc.ToLocalTime(); }
        }

        public virtual OrderStatus Status
        {
            get { return _status; }
            protected set { _status = value; }
        }

        public virtual Lessor Lessor
        {
            get { return _lessor; }
            protected set { _lessor = value; }
        }

        public virtual Lessee Lessee
        {
            get { return _lessee; }
            protected set { _lessee = value; }
        }

        public virtual Iesi.Collections.Generic.ISet<OrderItem> Items
        {
            get { return new ImmutableSet<OrderItem>(_items); }
            protected set { _items = value; }
        }

        public virtual decimal TotalPrice
        {
            get { return _items.Sum(x => x.ItemTotal); }
        }

        public virtual DateTime? LastToUtc
        {
            get
            {
                if (_items.Count == 0)
                    return null;
                return _items.Max(s => s.ToUtc);
            }
        }

        public virtual DateTime? FirstFromUtc
        {
            get
            {
                if (_items.Count == 0)
                    return null;
                return _items.Min(s => s.FromUtc);
            }
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

            Apply(new OrderRejected(initiator, OrderId, OrderShortId, Lessor, Lessee, (int) OrderStatus.Rejected,
                TotalPrice, CreateOrderEventItems()));
        }

        protected void When(OrderRejected e)
        {
            Status = OrderStatus.Rejected;
        }


        public virtual void Approve(Initiator initiator)
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Approved)
                throw new OrderAlreadyApprovedException();

            Apply(new OrderApproved(initiator, OrderId, OrderShortId, Lessor, Lessee, (int) OrderStatus.Approved,
                TotalPrice, CreateOrderEventItems()));
        }

        protected void When(OrderApproved e)
        {
            Status = OrderStatus.Approved;
        }


        public virtual void Close(Initiator initiator)
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();

            Apply(new OrderClosed(initiator, OrderId, OrderShortId, Lessor, Lessee, (int) OrderStatus.Closed,
                TotalPrice, CreateOrderEventItems()));
        }

        protected void When(OrderClosed e)
        {
            Status = OrderStatus.Closed;
        }


        public virtual void AddItem(Initiator initiator, OrderItemId orderItemId, Article article, DateTime fromUtc,
            DateTime toUtc, int quantity)
        {
            AssertPending();

            Apply(new OrderItemAdded(initiator, OrderId, OrderShortId, (int) Status, TotalPrice,
                new OrderEventItem(orderItemId, article.ArticleId, article.ArticleShortId,
                    article.Name, article.Price, fromUtc, toUtc, quantity, 0.0m)));
        }

        protected void When(OrderItemAdded e)
        {
            var item = new OrderItem(new OrderItemId(e.OrderItem.ItemId), new ArticleId(e.OrderItem.ArticleId),
                new ArticleShortId(e.OrderItem.ArticleShortId),
                e.OrderItem.Text, e.OrderItem.Period, e.OrderItem.Quantity, e.OrderItem.UnitPricePerWeek);
            _items.Add(item);
            item.Order = this;
        }

        public virtual void RemoveItem(Initiator initiator, Guid orderItemId)
        {
            AssertPending();

            var item = _items.FirstOrDefault(p => p.ItemId.Id == orderItemId);
            if (item == null)
                return;

            Apply(new OrderItemRemoved(initiator, OrderId, OrderShortId,
                (int) Status, TotalPrice, CreateOrderEventItem(item)));
        }

        protected void When(OrderItemRemoved e)
        {
            var item = _items.FirstOrDefault(p => p.ItemId.Id == e.OrderItem.ItemId);
            if (item == null)
                return;

            _items.Remove(item);
            item.Order = null;
        }


        public virtual void ChangeAmount(Initiator initiator, Guid orderItemId, int amount)
        {
            AssertPending();

            var item = _items.SingleOrDefault(p => p.ItemId.Id == orderItemId);
            if (item == null)
                return;

            Apply(new OrderItemQuantityChanged(initiator, OrderId, OrderShortId,
                (int) Status, TotalPrice, item.ItemId, item.Amount, amount,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemQuantityChanged e)
        {
            var item = _items.SingleOrDefault(p => p.ItemId.Id == e.OrderItemId);
            if (item == null)
                return;

            item.ChangeAmount(e.NewQuantity);
        }


        public virtual void ChangeItemPeriod(Initiator initiator, Guid orderItemId, DateTime fromUtc, DateTime toUtc)
        {
            AssertPending();

            var item = _items.SingleOrDefault(p => p.ItemId.Id == orderItemId);
            if (item == null)
                return;

            Apply(new OrderItemPeriodChanged(initiator, OrderId, OrderShortId, (int) Status, TotalPrice, orderItemId,
                new Period(item.FromUtc, item.ToUtc), new Period(fromUtc, toUtc), CreateOrderEventItem(item)));
        }

        protected void When(OrderItemPeriodChanged e)
        {
            var item = _items.SingleOrDefault(p => p.ItemId.Id == e.OrderItemId);
            if (item == null)
                return;

            item.ChangePeriod(e.NewPeriod.FromUtc, e.NewPeriod.ToUtc);
        }


        public virtual void ChangeItemTotal(Initiator initiator, Guid orderItemId, decimal itemTotal)
        {
            AssertPending();

            var item = _items.SingleOrDefault(p => p.ItemId.Id == orderItemId);
            if (item == null)
                return;

            Apply(new OrderItemTotalChanged(initiator, OrderId, OrderShortId,
                (int) Status, TotalPrice, item.ItemId, item.LineTotal, itemTotal,
                CreateOrderEventItem(item)));
        }

        protected void When(OrderItemTotalChanged e)
        {
            var item = _items.SingleOrDefault(p => p.ItemId.Id == e.OrderItemId);
            if (item == null)
                return;

            item.ChangeTotal(e.NewItemTotal);
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

        private IList<OrderEventItem> CreateOrderEventItems()
        {
            return Items.Select(CreateOrderEventItem).ToList();
        }

        private OrderEventItem CreateOrderEventItem(OrderItem item)
        {
            return new OrderEventItem(item.ItemId, item.ArticleId, item.ArticleShortId, item.Text, item.UnitPrice,
                item.FromUtc, item.ToUtc, item.Amount, item.ItemTotal);
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