namespace Phundus.Shop.Orders.Model
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Shop.Model;

    public class OrderItem
    {
        private int _amount;
        private ArticleShortId _articleShortId;
        private DateTime _fromUtc;
        private OrderItemId _itemId;
        private Order _order;
        private string _text;
        private DateTime _toUtc;
        private decimal _unitPrice;
        private decimal _itemTotal;
        private ArticleId _articleId;

        public OrderItem(OrderItemId orderItemId, ArticleId articleId, ArticleShortId articleShortId, string text, Period period, int quantity, decimal unitPricePerWeek)
        {
            if (orderItemId == null) throw new ArgumentNullException("orderItemId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (period == null) throw new ArgumentNullException("period");

            _itemId = orderItemId;
            _articleId = articleId;
            _articleShortId = articleShortId;
            _text = text;
            _fromUtc = period.FromUtc;
            _toUtc = period.ToUtc;
            _amount = quantity;
            _unitPrice = unitPricePerWeek;

            CalculateTotal();
        }

        public OrderItem(Order order, OrderItemId orderItemId, Article article, DateTime fromUtc, DateTime toUtc, int amount)
        {
            if (orderItemId == null) throw new ArgumentNullException("orderItemId");
            if (article == null) throw new ArgumentNullException("article");

            _itemId = orderItemId;
            _order = order;
            _articleId = article.ArticleId;
            _articleShortId = article.ArticleShortId;
            _unitPrice = article.Price;
            _text = article.Name;
            _fromUtc = fromUtc;
            _toUtc = toUtc;
            _amount = amount;

            CalculateTotal();
        }

        protected OrderItem()
        {
        }

        public virtual OrderItemId ItemId
        {
            get { return _itemId; }
            protected set { _itemId = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual Order Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public virtual int Amount
        {
            get { return _amount; }
            protected set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", @"Die Menge darf nicht kleiner als Eins sein.");
                _amount = value;
            }
        }

        public virtual DateTime FromUtc
        {
            get { return _fromUtc; }
            protected set { _fromUtc = value; }
        }

        public virtual DateTime FromLocal
        {
            get { return _fromUtc.ToLocalTime(); }
        }

        public virtual DateTime ToUtc
        {
            get { return _toUtc; }
            protected set { _toUtc = value; }
        }

        public virtual DateTime ToLocal
        {
            get { return _toUtc.ToLocalTime(); }
        }

        public virtual ArticleShortId ArticleShortId
        {
            get { return _articleShortId; }
            protected set { _articleShortId = value; }
        }

        public virtual ArticleId ArticleId
        {
            get { return _articleId;  }
            protected set { _articleId = value; }
        }

        public virtual string Text
        {
            get { return _text; }
            protected set { _text = value; }
        }

        public virtual decimal UnitPrice
        {
            get { return _unitPrice; }
            protected set { _unitPrice = value; }
        }

        public virtual decimal ItemTotal
        {
            get { return _itemTotal; }
            protected set { _itemTotal = value; }
        }

        /// <summary>
        /// Wird für E-Mail-Template benötigt!
        /// </summary>
        public virtual decimal LineTotal { get { return ItemTotal; } }

        private void CalculateTotal()
        {
            var priceInfo = new PerDayWithPerSevenDaysPricePricingStrategy()
                .Calculate(new Period(FromUtc, ToUtc), Amount, UnitPrice);
            _itemTotal = priceInfo.Price;
        }
        
        public virtual void ChangeAmount(int amount)
        {
            Amount = amount;
            CalculateTotal();
        }

        public virtual void ChangePeriod(DateTime fromUtc, DateTime toUtc)
        {
            FromUtc = fromUtc;
            ToUtc = toUtc;
            CalculateTotal();
        }

        public virtual void ChangeTotal(decimal itemTotal)
        {
            ItemTotal = itemTotal;
        }
    }
}