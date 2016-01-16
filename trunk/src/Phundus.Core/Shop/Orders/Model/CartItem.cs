namespace Phundus.Shop.Orders.Model
{
    using System;
    using Common.Domain.Model;
    using Ddd;
    using Pricing.Model;

    public class CartItem : EntityBase
    {
        private CartItemGuid _cartItemGuid = new CartItemGuid();
        public virtual Cart Cart { get; set; }

        public virtual CartItemGuid CartItemGuid
        {
            get { return _cartItemGuid; }
            protected set { _cartItemGuid = value; }
        }
        public virtual int Position { get; set; }
        public virtual Article Article { get; set; }

        public virtual int ArticleId
        {
            get { return Article.Id; }
        }

        public virtual int Quantity { get; set; }
        public virtual DateTime From { get; set; }
        public virtual DateTime To { get; set; }

        public virtual int Days
        {
            get
            {
                return new PerDayWithPerSevenDaysPricePricingStrategy().Calculate(From, To, Quantity, UnitPrice).Days;
            }
            protected set { }
        }

        public virtual string LineText
        {
            get { return Article.Caption; }
        }

        public virtual decimal UnitPrice
        {
            get { return Article.Price; }
        }

        public virtual decimal ItemTotal
        {
            get
            {
                return new PerDayWithPerSevenDaysPricePricingStrategy().Calculate(From, To, Quantity, UnitPrice).Price;
            }
            protected set { }
        }

        public virtual bool IsAvailable { get; set; }
        public virtual Guid CartGuid { get; set; }

        public virtual void ChangeQuantity(int quantity)
        {
            if (quantity <= 0)
                return;
            Quantity = quantity;
        }

        public virtual void ChangePeriod(DateTime fromUtc, DateTime toUtc)
        {
            if (To < From)
                return;
            From = fromUtc;
            To = toUtc;
        }
    }
}