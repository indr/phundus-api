namespace Phundus.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.Shop;

    public interface ICartQueries
    {
        ICart FindByUserGuid(InitiatorId initiatorId, UserId userId);
    }

    public class CartsProjection : ProjectionBase<CartViewRow>, ICartQueries
    {
        public ICart FindByUserGuid(InitiatorId initiatorId, UserId userId)
        {
            if (initiatorId.Id != userId.Id)
                throw new AuthorizationException();

            return Single(p => p.UserId == userId.Id);
        }
    }
    
    public class CartViewRow : ICart
    {
        public virtual Guid CartId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual IList<ICartItem> Items { get; protected set; }

        public virtual decimal Total
        {
            get
            {
                if (Items == null)
                    return 0;
                return Items.Sum(s => s.ItemTotal);
            }
        }
    }

    public class CartItemViewRow : ICartItem
    {
        public virtual Guid CartItemGuid { get; protected set; }

        public virtual int Position { get; protected set; }
        public virtual int ArticleId { get; protected set; }
        public virtual Guid ArticleGuid { get; protected set; }
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