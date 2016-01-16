namespace Phundus.Shop.Orders.Model
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Pricing.Model;

    public class OrderItem
    {
        private int _amount;
        private int _articleId;
        private DateTime _fromUtc;
        private Guid _id = Guid.NewGuid();
        private Order _order;
        private string _text;
        private DateTime _toUtc;
        private decimal _unitPrice;
        private decimal _itemTotal;

        public OrderItem(Order order, OrderItem copyFrom)
        {
            if (order == null) throw new ArgumentNullException("order");
            if (copyFrom == null) throw new ArgumentNullException("copyFrom");

            _order = order;
            _articleId = copyFrom.ArticleId;
            _text = copyFrom.Text;
            _fromUtc = copyFrom.FromUtc;
            _toUtc = copyFrom.ToUtc;
            _amount = copyFrom.Amount;
            _unitPrice = copyFrom.UnitPrice;

            CalculateTotal();
        }

        public OrderItem(ArticleId articleId, string text, Period period, int quantity, decimal unitPricePerWeek)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (period == null) throw new ArgumentNullException("period");
            
            _articleId = articleId.Id;
            _text = text;
            _fromUtc = period.FromUtc;
            _toUtc = period.ToUtc;
            _amount = quantity;
            _unitPrice = unitPricePerWeek;

            CalculateTotal();
        }

        public OrderItem(Order order, Article article, DateTime fromUtc, DateTime toUtc, int amount)
        {
            AssertionConcern.AssertArgumentNotNull(article, "Article must be provided.");

            _order = order;
            _articleId = article.Id;
            _unitPrice = article.Price;
            _text = article.Caption;
            _fromUtc = fromUtc;
            _toUtc = toUtc;
            _amount = amount;

            CalculateTotal();
        }

        protected OrderItem()
        {
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual Order Order
        {
            get { return _order; }
            protected set { _order = value; }
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

        public virtual int ArticleId
        {
            get { return _articleId; }
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
                .Calculate(FromUtc.ToLocalTime(), ToUtc.ToLocalTime(), Amount, UnitPrice);
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

        public virtual void Delete()
        {
            _order = null;
        }

        public virtual void ChangeTotal(decimal itemTotal)
        {
            ItemTotal = itemTotal;
        }
    }
}