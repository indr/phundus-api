using System;
using Iesi.Collections.Generic;

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

        public ISet<OrderItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public bool AddItem(OrderItem item)
        {
            var result = Items.Add(item);
            item.Order = this;
            return result;
        }

        public bool RemoveItem(OrderItem item)
        {
            var result = Items.Remove(item);
            item.Order = null;
            return result;
        }
    }
}