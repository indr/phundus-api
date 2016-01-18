namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Contracts.Model;
    using Ddd;
    using Iesi.Collections.Generic;

    public class Order
    {
        private DateTime _createdUtc = DateTime.UtcNow;
        private Guid _guid = System.Guid.NewGuid();
        private Iesi.Collections.Generic.ISet<OrderItem> _items = new HashedSet<OrderItem>();
        private Lessee _lessee;
        private Lessor _lessor;
        private UserId _modifiedBy;
        private DateTime? _modifiedUtc;
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

        public virtual int Id { get; protected set; }

        public virtual Guid Guid
        {
            get { return _guid; }
            protected set { _guid = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual Lessor Lessor
        {
            get { return _lessor; }
            protected set { _lessor = value; }
        }

        public virtual DateTime CreatedUtc
        {
            get { return _createdUtc; }
            set { _createdUtc = value; }
        }

        public virtual DateTime CreatedLocal
        {
            get { return _createdUtc.ToLocalTime(); }
        }

        public virtual OrderStatus Status
        {
            get { return _status; }
            protected set { _status = value; }
        }

        public virtual DateTime? ModifiedUtc
        {
            get { return _modifiedUtc; }
            protected set { _modifiedUtc = value; }
        }

        public virtual UserId ModifiedBy
        {
            get { return _modifiedBy; }
            protected set { _modifiedBy = value; }
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

        public virtual OrderId OrderId
        {
            get { return new OrderId(Id); }
        }

        public virtual void Reject(UserId initiatorId)
        {
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();

            ModifiedBy = initiatorId;
            ModifiedUtc = DateTime.UtcNow;
            Status = OrderStatus.Rejected;

            EventPublisher.Publish(new OrderRejected {OrderId = Id});
        }

        public virtual void Approve(UserId initiatorId)
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Approved)
                throw new OrderAlreadyApprovedException();

            ModifiedBy = initiatorId;
            ModifiedUtc = DateTime.UtcNow;
            Status = OrderStatus.Approved;

            EventPublisher.Publish(new OrderApproved {OrderId = Id});
        }

        public virtual void Close(UserId initiatorId)
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();

            ModifiedBy = initiatorId;
            ModifiedUtc = DateTime.UtcNow;
            Status = OrderStatus.Closed;

            EventPublisher.Publish(new OrderClosed {OrderId = Id});
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

        public virtual void AddItem(OrderItemId orderItemId, Article article, DateTime fromUtc, DateTime toUtc, int quantity)
        {
            EnsurePending();

            var item = new OrderItem(this, orderItemId, article, fromUtc, toUtc, quantity);
            _items.Add(item);

            EventPublisher.Publish(new OrderItemAdded());
        }

        public virtual void RemoveItem(Guid orderItemId)
        {
            EnsurePending();

            var item = _items.FirstOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            _items.Remove(item);
            item.Delete();

            EventPublisher.Publish(new OrderItemRemoved());
        }

        public virtual void ChangeAmount(Guid orderItemId, int amount)
        {
            EnsurePending();

            var item = _items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            item.ChangeAmount(amount);

            EventPublisher.Publish(new OrderItemAmountChanged());
        }

        public virtual void ChangeItemPeriod(Guid orderItemId, DateTime fromUtc, DateTime toUtc)
        {
            EnsurePending();

            var item = _items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            item.ChangePeriod(fromUtc, toUtc);

            EventPublisher.Publish(new OrderItemPeriodChanged());
        }

        public virtual void ChangeItemTotal(Guid orderItemId, decimal itemTotal)
        {
            EnsurePending();

            var item = _items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            item.ChangeTotal(itemTotal);

            EventPublisher.Publish(new OrderItemTotalChanged());
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

    public class ArticleNotAvailableException : Exception
    {
        public ArticleNotAvailableException(OrderItem orderItem)
            : base("Die gewünschte Menge ist im gewünschten Zeitraum nicht vorhanden.")
        {
            OrderItemId = orderItem.Id;
        }

        public Guid OrderItemId { get; set; }
    }
}