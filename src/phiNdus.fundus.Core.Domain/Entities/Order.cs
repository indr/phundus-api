using System;
using Iesi.Collections.Generic;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Order : BaseEntity
    {
        private DateTime _createDate;
        private ISet<OrderItem> _items = new HashedSet<OrderItem>();

        public Order()
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

        public virtual User Approver { get; protected set; }

        public virtual DateTime? ApproveDate { get; protected set; }

        public virtual User Rejecter { get; protected set; }

        public virtual DateTime? RejectDate { get; protected set; }

        public virtual bool AddItem(OrderItem item)
        {
            var result = Items.Add(item);
            item.Order = this;
            return result;
        }

        public virtual bool RemoveItem(OrderItem item)
        {
            var result = Items.Remove(item);
            item.Order = null;
            return result;
        }

        public virtual void Approve(User approver)
        {
            Guard.Against<ArgumentNullException>(approver == null, "approver");

            Guard.Against<InvalidOperationException>(Status == OrderStatus.Approved,
                                                     "Die Bestellung wurde bereits bewilligt.");
            Guard.Against<InvalidOperationException>(Status == OrderStatus.Rejected,
                                                     "Die Bestellung wurde bereits abgelehnt.");

            Status = OrderStatus.Approved;
            Approver = approver;
            ApproveDate = DateTime.Now;
        }

        public virtual void Reject(User rejecter)
        {
            Guard.Against<ArgumentNullException>(rejecter == null, "rejecter");

            Guard.Against<InvalidOperationException>(Status == OrderStatus.Approved,
                                                     "Die Bestellung wurde bereits bewilligt.");
            Guard.Against<InvalidOperationException>(Status == OrderStatus.Rejected,
                                                     "Die Bestellung wurde bereits abgelehnt.");

            Status = OrderStatus.Rejected;
            Rejecter = rejecter;
            RejectDate = DateTime.Now;
        }
    }
}