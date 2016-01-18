namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Inventory.Services;

    public class Cart : Aggregate<CartId>
    {
        private ISet<CartItem> _items = new HashedSet<CartItem>();
        private Guid _userGuid;

        public Cart(InitiatorId initiatorId, UserId userId) : base(new CartId())
        {
            _userGuid = userId.Id;
        }

        protected Cart()
        {
        }

        public virtual ISet<CartItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual bool AreItemsAvailable
        {
            get { return Items.Count(p => p.IsAvailable == false) == 0; }
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

        public virtual CartItemId AddItem(Article article, DateTime fromUtc, DateTime toUtc, int quantity)
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
            return item.CartItemId;
        }

        protected virtual void AddItem(CartItem item)
        {
            Items.Add(item);
            item.Cart = this;
            item.CartGuid = Id.Id;
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
            var item = Items.SingleOrDefault(p => p.CartItemId.Id == cartItemGuid);
            if (item == null)
                return;

            item.ChangeQuantity(quantity);
            item.ChangePeriod(fromUtc, toUtc);
        }
    }
}