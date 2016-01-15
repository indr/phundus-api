namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Inventory.Services;
    using Repositories;
    using Shop.Services;

    public class Cart : EntityBase
    {
        private Guid _cartGuid = Guid.NewGuid();
        private Iesi.Collections.Generic.ISet<CartItem> _items = new HashedSet<CartItem>();
        private Guid _userGuid;
        private int _userId;

        public Cart(InitiatorGuid initiatorGuid, UserGuid userGuid)
        {
            _userGuid = userGuid.Id;
        }

        protected Cart()
        {
        }

        public virtual Guid CartGuid
        {
            get { return _cartGuid; }
            protected set { _cartGuid = value; }
        }

        public virtual Iesi.Collections.Generic.ISet<CartItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual bool AreItemsAvailable
        {
            get { return Items.Count(p => p.IsAvailable == false) == 0; }
        }

        [Obsolete]
        public virtual int CustomerId
        {
            get { return _userId; }
            protected set { _userId = value; }
        }

        public virtual Guid UserGuid
        {
            get { return _userGuid; }
            protected set { _userGuid = value; }
        }

        public virtual bool IsEmpty
        {
            get { return _items.IsEmpty; }
        }


        public virtual CartItemGuid AddItem(Article article, DateTime fromUtc, DateTime toUtc, int quantity)
        {
            var item = new CartItem();
            item.Position = 1;
            if (Items.Count > 0)
                item.Position = Items.Max(s => s.Position) + 1;
            item.Article = article;
            item.Quantity = quantity;
            item.From = fromUtc;
            item.To = toUtc;

            AddItem(item);
            return item.CartItemGuid;
        }

        protected virtual void AddItem(CartItem item)
        {
            Items.Add(item);
            item.Cart = this;
            item.CartGuid = CartGuid;
        }

        public virtual void RemoveItem(CartItemGuid cartItemGuid)
        {
            var item = Items.SingleOrDefault(p => Equals(p.CartItemGuid, cartItemGuid));
            if (item == null)
                return;
            RemoveItem(item);
        }

        protected virtual void RemoveItem(CartItem item)
        {
            item.Cart = null;
            Items.Remove(item);
        }

        public virtual void CalculateAvailability(IAvailabilityService availabilityService)
        {
            foreach (var each in Items)
            {
                each.IsAvailable = availabilityService.IsArticleAvailable(each.ArticleId, each.From, each.To,
                    each.Quantity, Guid.Empty);
            }
        }

        public virtual void Clear()
        {
            CartItem item;
            while (null != (item = Items.FirstOrDefault()))
            {
                RemoveItem(item);
            }
        }

        public virtual void UpdateItem(Guid cartItemGuid, int quantity, DateTime fromUtc, DateTime toUtc)
        {
            var item = Items.SingleOrDefault(p => p.CartItemGuid.Id == cartItemGuid);
            if (item == null)
                return;

            item.ChangeQuantity(quantity);
            item.ChangePeriod(fromUtc, toUtc);
        }
    }
}