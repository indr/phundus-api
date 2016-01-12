namespace Phundus.Shop.Queries
{
    using System;
    using Integration.Shop;

    public class CartItemViewRow : ICartItem
    {
        public virtual int CartItemId { get; protected set; }
        public virtual Guid CartItemGuid { get; protected set; }

        public virtual int ArticleId { get; protected set; }
        public virtual string Text { get; protected set; }
        public virtual DateTime FromUtc { get; protected set; }
        public virtual DateTime ToUtc { get; protected set; }
        public virtual int Quantity { get; protected set; }
    }
}