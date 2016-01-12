namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using Common.Domain.Model;
    using Ddd;
    using Pricing.Model;

    public class CartItem : EntityBase
    {
        private CartItemId _cartItemId = new CartItemId();
        public virtual Cart Cart { get; set; }

        public virtual CartItemId CartItemId
        {
            get { return _cartItemId; }
            protected set { _cartItemId = value; }
        }

        public virtual Article Article { get; set; }

        public virtual int ArticleId
        {
            get { return Article.ArticleId; }
        }

        public virtual int Quantity { get; set; }
        public virtual DateTime From { get; set; }
        public virtual DateTime To { get; set; }

        public virtual string LineText
        {
            get { return Article.Caption; }
        }

        public virtual decimal UnitPrice
        {
            get { return Article.Price; }
        }

        public virtual decimal LineTotal
        {
            get
            {
                return new PerDayWithPerSevenDaysPricePricingStrategy().Calculate(From, To, Quantity, UnitPrice).Price;
            }
        }

        public virtual bool IsAvailable { get; set; }
        public virtual Guid CartGuid { get; set; }
    }
}