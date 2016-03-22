namespace Phundus.Shop.Model
{
    using System;
    using Common.Domain.Model;
    using Products;

    public class CartItem : EntityBase
    {
        private CartItemId _cartItemId = new CartItemId();

        public CartItem(CartItemId cartItemId)
        {
            _cartItemId = cartItemId;
        }

        protected CartItem()
        {
        }

        public virtual Cart Cart { get; set; }

        public virtual CartItemId CartItemId
        {
            get { return _cartItemId; }
            protected set { _cartItemId = value; }
        }

        public virtual int Position { get; set; }

        public virtual Article Article { get; set; }

        public virtual ArticleId ArticleId
        {
            get { return Article.ArticleId; }
        }

        public virtual ArticleShortId ArticleShortId
        {
            get { return Article.ArticleShortId; }
        }

        public virtual StoreId StoreId
        {
            get { return Article.StoreId; }
        }

        public virtual int Quantity { get; set; }

        public virtual Period Period
        {
            get { return new Period(From, To); }
        }


        public virtual DateTime From { get; set; }

        public virtual DateTime To { get; set; }

        public virtual int Days
        {
            get
            {
                return
                    new PerDayWithPerSevenDaysPricePricingStrategy().Calculate(new Period(From, To), Quantity, UnitPrice)
                        .Days;
            }
            protected set
            {
                // Noop for NHibenrate
            }
        }

        public virtual string LineText
        {
            get { return Article.Name; }
        }

        public virtual decimal UnitPrice
        {
            get { return Article.Price; }
        }

        public virtual decimal ItemTotal
        {
            get
            {
                var price = new PerDayWithPerSevenDaysPricePricingStrategy().Calculate(new Period(From, To), Quantity,
                    UnitPrice);
                return price.Price;
            }
            protected set
            {
                // Noop for NHibernate
            }
        }

        public virtual LessorId LessorId
        {
            get { return Article.LessorId; }
        }


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