namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Shop.Model;

    public class Order
    {
        private DateTime _createdAtUtc = DateTime.UtcNow;
        private Iesi.Collections.Generic.ISet<OrderItem> _items = new HashedSet<OrderItem>();
        private Lessee _lessee;
        private Lessor _lessor;
        private OrderId _orderId = new OrderId();
        private ShortOrderId _shortOrderId = new ShortOrderId(0);
        private OrderStatus _status = OrderStatus.Pending;

        public Order(Lessor lessor, Lessee lessee) : this(lessor, lessee, null)
        {
        }

        public Order(Lessor lessor, Lessee lessee, ICollection<OrderItem> items)
        {
            if (lessor == null) throw new ArgumentNullException("lessor");
            if (lessee == null) throw new ArgumentNullException("lessee");
            _lessor = lessor;
            _lessee = lessee;

            if ((items != null) && (items.Count > 0))
                _items.AddAll(items.Select(s => new OrderItem(this, s)).ToList());
        }

        protected Order()
        {
        }

        public virtual int Id
        {
            get { return _shortOrderId.Id; }
            protected set { _shortOrderId = new ShortOrderId(value); }
        }

        public virtual ShortOrderId ShortOrderId
        {
            get { return _shortOrderId; }
            protected set { _shortOrderId = value; }
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

        public virtual void Reject(Initiator initiator)
        {
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();

            Status = OrderStatus.Rejected;

            EventPublisher.Publish(new OrderRejected(initiator, OrderId, ShortOrderId, Lessor, Lessee, (int) Status,
                TotalPrice, CreateOrderEventItems()));
        }

        public virtual void Approve(Initiator initiator)
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Approved)
                throw new OrderAlreadyApprovedException();

            Status = OrderStatus.Approved;

            EventPublisher.Publish(new OrderApproved(initiator, OrderId, ShortOrderId, Lessor, Lessee, (int) Status,
                TotalPrice, CreateOrderEventItems()));
        }

        public virtual void Close(Initiator initiator)
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();

            Status = OrderStatus.Closed;

            EventPublisher.Publish(new OrderClosed(initiator, OrderId, ShortOrderId, Lessor, Lessee, (int) Status,
                TotalPrice, CreateOrderEventItems()));
        }

        public virtual void EnsurePending()
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

        public virtual void AddItem(Initiator initiator, OrderItemId orderItemId, Article article, DateTime fromUtc,
            DateTime toUtc,
            int quantity)
        {
            EnsurePending();

            var item = new OrderItem(this, orderItemId, article, fromUtc, toUtc, quantity);
            _items.Add(item);

            EventPublisher.Publish(new OrderItemAdded(initiator, OrderId, ShortOrderId,
                (int) Status, TotalPrice, CreateOrderEventItem(item)));
        }

        public virtual void RemoveItem(Initiator initiator, Guid orderItemId)
        {
            EnsurePending();

            var item = _items.FirstOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            _items.Remove(item);
            item.Delete();

            EventPublisher.Publish(new OrderItemRemoved(initiator, OrderId, ShortOrderId,
                (int) Status, TotalPrice, CreateOrderEventItem(item)));
        }

        public virtual void ChangeAmount(Initiator initiator, Guid orderItemId, int amount)
        {
            EnsurePending();

            var item = _items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            var oldQuantity = item.Amount;
            item.ChangeAmount(amount);

            EventPublisher.Publish(new OrderItemQuantityChanged(initiator, OrderId, ShortOrderId,
                (int) Status, TotalPrice, item.Id, oldQuantity, item.Amount,
                CreateOrderEventItem(item)));
        }

        public virtual void ChangeItemPeriod(Initiator initiator, Guid orderItemId, DateTime fromUtc, DateTime toUtc)
        {
            EnsurePending();

            var item = _items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            item.ChangePeriod(fromUtc, toUtc);

            EventPublisher.Publish(new OrderItemPeriodChanged());
        }

        public virtual void ChangeItemTotal(Initiator initiator, Guid orderItemId, decimal itemTotal)
        {
            EnsurePending();

            var item = _items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            var oldItemTotal = item.ItemTotal;
            item.ChangeTotal(itemTotal);

            EventPublisher.Publish(new OrderItemTotalChanged(initiator, OrderId, ShortOrderId,
                (int) Status, TotalPrice, item.Id, oldItemTotal, item.ItemTotal,
                CreateOrderEventItem(item)));
        }

        private IList<OrderEventItem> CreateOrderEventItems()
        {
            return Items.Select(CreateOrderEventItem).ToList();
        }

        private OrderEventItem CreateOrderEventItem(OrderItem item)
        {
            return new OrderEventItem(item.Id, item.ArticleId, item.ArticleShortId, item.Text, item.UnitPrice,
                item.FromUtc, item.ToUtc, item.Amount, item.ItemTotal);
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