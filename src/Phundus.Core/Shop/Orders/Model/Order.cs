namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Linq;
    using Contracts.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Inventory.Model;
    using Inventory.Repositories;
    using Inventory._Legacy;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;

    public class Order
    {
        private Borrower _borrower;
        private DateTime _createdOn = DateTime.UtcNow;
        private ISet<OrderItem> _items = new HashedSet<OrderItem>();
        private int? _modifiedBy;
        private DateTime? _modifiedOn;
        private int _organizationId;
        private OrderStatus _status = OrderStatus.Pending;

        protected Order()
        {
        }

        public Order(int organizationId, Borrower borrower)
        {
            _organizationId = organizationId;
            _borrower = borrower;
        }

        public virtual int Id { get; protected set; }

        public virtual int Version { get; protected set; }

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual DateTime CreatedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }

        public virtual OrderStatus Status
        {
            get { return _status; }
            protected set { _status = value; }
        }

        public virtual DateTime? ModifiedOn
        {
            get { return _modifiedOn; }
            protected set { _modifiedOn = value; }
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

        public virtual DateTime? LastTo
        {
            get
            {
                if (_items.Count == 0)
                    return null;
                return _items.Max(s => s.To);
            }
        }

        public virtual DateTime? FirstFrom
        {
            get
            {
                if (_items.Count == 0)
                    return null;
                return _items.Min(s => s.From);
            }
        }

        public virtual void Reject(int initiatorId)
        {
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();

            ModifiedBy = initiatorId;
            ModifiedOn = DateTime.UtcNow;
            Status = OrderStatus.Rejected;

            EventPublisher.Publish(new OrderRejected { OrderId = Id });
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
            ModifiedOn = DateTime.UtcNow;
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
            ModifiedOn = DateTime.UtcNow;
            Status = OrderStatus.Closed;

            EventPublisher.Publish(new OrderClosed { OrderId = Id });
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

        public virtual bool AddItem(OrderItem item, ISession session)
        {
            EnsurePending();

            var checker = new AvailabilityChecker(item.Article, session);
            if (!checker.Check(item.From, item.To, item.Amount))
                throw new ArticleNotAvailableException(item);

            return _items.Add(item);
        }

        public virtual OrderItem AddItem(Article article, DateTime from, DateTime to, int amount)
        {
            EnsurePending();

            var item = new OrderItem(this)
            {
                Article = article,
                From = @from,
                To = to,
                Amount = amount
            };

            _items.Add(item);

            EventPublisher.Publish(new OrderItemAdded());

            return item;
        }

        public virtual bool AddItem(int articleId, int amount, DateTime begin, DateTime end, ISession session)
        {
            EnsurePending();
            var article = ServiceLocator.Current.GetInstance<IArticleRepository>().ById(articleId);

            var item = new OrderItem(this)
            {
                Article = article,
                Amount = amount,
                From = begin,
                To = end
            };

            return AddItem(item, session);
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

        public virtual void ChangeItemPeriod(Guid orderItemId, DateTime @from, DateTime to)
        {
            EnsurePending();

            var item = _items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            item.ChangePeriod(@from, to);

            EventPublisher.Publish(new OrderItemPeriodChanged());
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