namespace Phundus.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Integration.Shop;

    public class CartViewRow : ICart
    {
        public virtual int CartId { get; protected set; }
        public virtual Guid CartGuid { get; protected set; }
        public virtual int UserId { get; protected set; }
        public virtual Guid UserGuid { get; protected set; }
        public virtual IList<ICartItem> Items { get; protected set; }
    }
}