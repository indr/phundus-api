﻿namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Linq;
    using Contracts.Model;
    using Ddd;
    using IdentityAndAccess.Users.Model;
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
        private int _id;
        private ISet<OrderItem> _items = new HashedSet<OrderItem>();
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

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

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

        public virtual Borrower Borrower
        {
            get { return _borrower; }
            protected set { _borrower = value; }
        }

        public virtual int ReserverId
        {
            get { return Borrower.Id; }
            protected set { }
        }

        public virtual ISet<OrderItem> Items
        {
            get { return new ImmutableSet<OrderItem>(_items); }
            protected set { _items = value; }
        }

        public virtual User Modifier { get; protected set; }

        public virtual DateTime? ModifyDate { get; protected set; }

        public virtual decimal TotalPrice
        {
            get { return Items.Sum(x => x.LineTotal); }
        }

        public virtual DateTime LastTo
        {
            get { return Items.Max(s => s.To); }
        }

        public virtual DateTime FirstFrom
        {
            get { return Items.Min(s => s.From); }
        }

        public virtual void Reject()
        {
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();

            Status = OrderStatus.Rejected;

            EventPublisher.Publish(new OrderRejected());
        }

        public virtual void Approve()
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();
            if (Status == OrderStatus.Approved)
                throw new OrderAlreadyApprovedException();

            Status = OrderStatus.Approved;

            EventPublisher.Publish(new OrderApproved());
        }

        public virtual void Close()
        {
            if (Status == OrderStatus.Rejected)
                throw new OrderAlreadyRejectedException();
            if (Status == OrderStatus.Closed)
                throw new OrderAlreadyClosedException();

            Status = OrderStatus.Closed;

            EventPublisher.Publish(new OrderClosed());
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

            var result = Items.Add(item);
            item.Order = this;
            return result;
        }

        public virtual OrderItem AddItem(Article article, DateTime from, DateTime to, int amount)
        {
            EnsurePending();

            var item = new OrderItem();
            item.Article = article;
            item.From = from;
            item.To = to;
            item.Amount = amount;

            item.Order = this;
            Items.Add(item);

            EventPublisher.Publish(new OrderItemAdded());

            return item;
        }

        public virtual bool AddItem(int articleId, int amount, DateTime begin, DateTime end, ISession session)
        {
            EnsurePending();

            var item = new OrderItem();
            item.Article = ServiceLocator.Current.GetInstance<IArticleRepository>().ById(articleId);
            item.Amount = amount;
            item.From = begin;
            item.To = end;
            return AddItem(item, session);
        }

        public virtual void RemoveItem(Guid orderItemId)
        {
            EnsurePending();

            var item = Items.FirstOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            Items.Remove(item);
            item.Order = null;

            EventPublisher.Publish(new OrderItemRemoved());
        }

        public virtual void ChangeAmount(Guid orderItemId, int amount)
        {
            EnsurePending();

            var item = Items.SingleOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return;

            item.ChangeAmount(amount);

            EventPublisher.Publish(new OrderItemAmountChanged());
        }

        public virtual void ChangeItemPeriod(Guid orderItemId, DateTime @from, DateTime to)
        {
            EnsurePending();

            var item = Items.SingleOrDefault(p => p.Id == orderItemId);
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