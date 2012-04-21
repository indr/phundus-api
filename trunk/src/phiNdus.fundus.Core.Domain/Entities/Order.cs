using System;
using System.Linq;
using Iesi.Collections.Generic;
using log4net;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Order : Entity
    {
        private DateTime _createDate;
        private ISet<OrderItem> _items = new HashedSet<OrderItem>();

        public Order() : this(0, 0)
        {
        }

        public Order(int id, int version) : base(id, version)
        {
            _createDate = DateTime.Now;
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public virtual ISet<OrderItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual User Reserver { get; set; }

        public virtual OrderStatus Status { get; protected set; }

        public virtual User Modifier { get; protected set; }

        public virtual DateTime? ModifyDate { get; protected set; }

        public virtual double TotalPrice { get { return Items.Sum(x => x.LineTotal); } }

        public virtual bool AddItem(OrderItem item)
        {
            var result = Items.Add(item);
            item.Order = this;
            return result;
        }

        public virtual bool AddItem(int articleId, int amount, DateTime begin, DateTime end)
        {
            var item = new OrderItem();
            item.Article = IoC.Resolve<IArticleRepository>().Get(articleId);
            item.Amount = amount;
            item.From = begin;
            item.To = end;
            return AddItem(item);
        }

        public virtual bool RemoveItem(OrderItem item)
        {
            var result = Items.Remove(item);
            item.Order = null;
            return result;
        }

        public virtual void Checkout()
        {
            Status = OrderStatus.Pending;
            LogManager.GetLogger(GetType()).Warn("Checkout() is not implemented!");
        }

        public virtual void Approve(User approver)
        {
            Guard.Against<ArgumentNullException>(approver == null, "approver");

            Guard.Against<InvalidOperationException>(Status == OrderStatus.Approved,
                                                     "Die Bestellung wurde bereits bewilligt.");
            Guard.Against<InvalidOperationException>(Status == OrderStatus.Rejected,
                                                     "Die Bestellung wurde bereits abgelehnt.");

            Status = OrderStatus.Approved;
            Modifier = approver;
            ModifyDate = DateTime.Now;
        }

        public virtual void Reject(User rejecter)
        {
            Guard.Against<ArgumentNullException>(rejecter == null, "rejecter");

            Guard.Against<InvalidOperationException>(Status == OrderStatus.Approved,
                                                     "Die Bestellung wurde bereits bewilligt.");
            Guard.Against<InvalidOperationException>(Status == OrderStatus.Rejected,
                                                     "Die Bestellung wurde bereits abgelehnt.");

            Status = OrderStatus.Rejected;
            Modifier = rejecter;
            ModifyDate = DateTime.Now;
        }
    }
}