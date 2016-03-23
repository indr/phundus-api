namespace Phundus.Shop.Model
{
    using System;
    using Common.Domain.Model;
    using Products;

    public class CartItem : EntityBase
    {
        private CartItemId _cartItemId = new CartItemId();
        private LessorId _lessorId;
        private decimal _unitPrice;
        private string _lineText;
        private StoreId _storeId;
        private ArticleShortId _articleShortId;
        private ArticleId _articleId;

        public CartItem(CartItemId cartItemId, Article article)
        {
            _cartItemId = cartItemId;

            _articleId = article.ArticleId;
            _articleShortId = article.ArticleShortId;
            _lessorId = article.LessorId;
            _storeId = article.StoreId;
            _unitPrice = article.Price;
            _lineText = article.Name;
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

        public virtual ArticleId ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

        public virtual ArticleShortId ArticleShortId
        {
            get { return _articleShortId; }
            protected set { _articleShortId = value; }
        }

        public virtual StoreId StoreId
        {
            get { return _storeId; }
            protected set { _storeId = value; }
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
            get { return _lineText; }
            protected set { _lineText = value; }
        }

        public virtual decimal UnitPrice
        {
            get { return _unitPrice; }
            protected set { _unitPrice = value; }
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
            get { return _lessorId; }
            protected set { _lessorId = value; }
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