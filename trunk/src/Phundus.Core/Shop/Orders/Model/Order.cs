namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Contracts.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Inventory.Services;

    public class Order
    {
        private DateTime _createdUtc = DateTime.UtcNow;
        private Iesi.Collections.Generic.ISet<OrderItem> _items = new HashedSet<OrderItem>();
        private Lessee _lessee;
        private Lessor _lessor;
        private UserGuid _modifiedBy;
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

        public virtual UserGuid ModifiedBy
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

        public virtual void Reject(UserGuid initiatorId)
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

        public virtual void Approve(UserGuid initiatorId)
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

        public virtual void Close(UserGuid initiatorId)
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


        public virtual OrderItem AddItem(Article article, DateTime fromUtc, DateTime toUtc, int amount)
        {
            EnsurePending();

            var item = new OrderItem(this, article, fromUtc, toUtc, amount);
            _items.Add(item);

            EventPublisher.Publish(new OrderItemAdded());

            return item;
        }

        public virtual bool AddItem(Article article, int amount, DateTime fromUtc, DateTime toUtc,
            IAvailabilityService availabilityService)
        {
            EnsurePending();

            var item = new OrderItem(this, article, fromUtc, toUtc, amount);

            return AddItem(item, availabilityService);
        }

        private bool AddItem(OrderItem item, IAvailabilityService availabilityService)
        {
            EnsurePending();

            if (
                !availabilityService.IsArticleAvailable(item.ArticleId, item.FromUtc, item.ToUtc, item.Amount,
                    Guid.Empty))
                throw new ArticleNotAvailableException(item);

            return _items.Add(item);
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