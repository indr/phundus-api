namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Common.Projecting;

    public interface ICartsQueries
    {
        CartData FindByUserGuid(InitiatorId initiatorId, UserId userId);
    }

    public class CartsProjection : ProjectionBase<CartData>, ICartsQueries
    {
        public CartData FindByUserGuid(InitiatorId initiatorId, UserId userId)
        {
            if (initiatorId.Id != userId.Id)
                throw new AuthorizationException();

            return SingleOrDefault(p => p.UserId == userId.Id);
        }

        public override void Handle(DomainEvent e)
        {            
        }
    }

    public class CartData
    {
        public virtual Guid CartId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual IList<CartItemData> Items { get; protected set; }

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

    public class CartItemData 
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