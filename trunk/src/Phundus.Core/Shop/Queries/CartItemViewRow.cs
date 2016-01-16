namespace Phundus.Shop.Queries
{
    using System;
    using Integration.Shop;

    public class CartItemViewRow : ICartItem
    {
        public virtual Guid CartItemGuid { get; protected set; }

        public virtual int Position { get; protected set; }
        public virtual int ArticleId { get; protected set; }
        public virtual string Text { get; protected set; }
        public virtual DateTime FromUtc { get; protected set; }
        public virtual DateTime ToUtc { get; protected set; }
        public virtual int Quantity { get; protected set; }
        public virtual decimal UnitPricePerWeek { get; protected set; }
        public virtual decimal ItemTotal { get; protected set; }
        public virtual int Days { get; protected set; }

        public virtual Guid OwnerGuid { get; protected set; }
        public virtual string OwnerType { get; protected set; }
        public virtual string OwnerName { get; protected set; }
    }
}