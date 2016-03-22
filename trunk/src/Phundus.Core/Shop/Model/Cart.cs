namespace Phundus.Shop.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Iesi.Collections.Generic;    
    using Products;

    public class Cart : Aggregate<CartId>
    {
        private Iesi.Collections.Generic.ISet<CartItem> _items = new HashedSet<CartItem>();
        private UserId _userId;

        public Cart(UserId userId) : base(new CartId())
        {
            if (userId == null) throw new ArgumentNullException("userId");
            _userId = userId;
        }

        protected Cart()
        {
        }

        public virtual Iesi.Collections.Generic.ISet<CartItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual bool IsEmpty
        {
            get { return _items.IsEmpty; }
        }

        public virtual UserId UserId
        {
            get { return _userId; }
            protected set { _userId = value; }
        }

        public virtual CartItemId AddItem(Article product, Period period, int quantity)
        {
            return AddItem(new CartItemId(), product, period, quantity);
        }

        public virtual CartItemId AddItem(CartItemId cartItemId, Article article, Period period, int quantity)
        {
            var item = new CartItem(cartItemId);
            item.Position = 1;
            if (Items.Count > 0)
                item.Position = Items.Max(s => s.Position) + 1;
            item.Article = article;
            item.Quantity = quantity;
            item.From = period.FromUtc;
            item.To = period.ToUtc;

            AddItem(item);
            return item.CartItemId;
        }

        protected virtual void AddItem(CartItem item)
        {
            Items.Add(item);
            item.Cart = this;
        }

        public virtual void RemoveItem(CartItemId cartItemId)
        {
            var item = Items.SingleOrDefault(p => Equals(p.CartItemId, cartItemId));
            if (item == null)
                return;
            RemoveItem(item);
        }

        protected virtual void RemoveItem(CartItem item)
        {
            item.Cart = null;
            Items.Remove(item);
        }

        public virtual void Clear()
        {
            CartItem item;
            while (null != (item = Items.FirstOrDefault()))
            {
                RemoveItem(item);
            }
        }

        public virtual void ChangeQuantityAndPeriod(Guid cartItemGuid, int quantity, DateTime fromUtc, DateTime toUtc)
        {
            var item = Items.SingleOrDefault(p => p.CartItemId.Id == cartItemGuid);
            if (item == null)
                return;

            item.ChangeQuantity(quantity);
            item.ChangePeriod(fromUtc, toUtc);
        }

        public virtual ICollection<CartItem> TakeItems(LessorId lessorId)
        {
            var items = _items.Where(p => Equals(p.LessorId, lessorId)).ToList();
            items.ForEach(RemoveItem);
            return items;
        }
    }
}