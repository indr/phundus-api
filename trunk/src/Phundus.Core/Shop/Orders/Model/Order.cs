namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Linq;
    using Contracts.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Inventory.Services;

    public class Order
    {
        private Borrower _borrower;
        private DateTime _createdUtc = DateTime.UtcNow;
        private ISet<OrderItem> _items = new HashedSet<OrderItem>();
        private Lessor _lessor;
        private int? _modifiedBy;
        private DateTime? _modifiedUtc;
        private OrderStatus _status = OrderStatus.Pending;

        public Order(Lessor lessor, Borrower borrower)
        {
            _lessor = lessor;
            _borrower = borrower;
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

        public virtual int? ModifiedBy
        {
            get { return _modifiedBy; }
            protected set { _modifiedBy = value; }
        }

        public virtual Borrower Borrower
        {
            get { return _borrower; }
            protected set { _borrower = value; }
        }

        public virtual ISet<OrderItem> Items
        {
            get { return new ImmutableSet<OrderItem>(_items); }
            protected set { _items = value; }
        }

        public virtual decimal TotalPrice
        {
            get { return _items.Sum(x => x.LineTotal); }
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

        public virtual void Reject(int initiatorId)
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

        public virtual void Approve(int initiatorId)
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

        public virtual void Close(int initiatorId)
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

        public virtual bool AddItem(OrderItem item, IAvailabilityService availabilityService)
        {
            EnsurePending();

            if (
                !availabilityService.IsArticleAvailable(item.ArticleId, item.FromUtc, item.ToUtc, item.Amount,
                    Guid.Empty))
                throw new ArticleNotAvailableException(item);

            return _items.Add(item);
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