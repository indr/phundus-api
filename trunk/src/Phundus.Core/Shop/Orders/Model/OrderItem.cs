namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using Common.Domain.Model;
    using Inventory.Domain.Model.Catalog;
    using Pricing.Model;

    public class OrderItem
    {
        private int _amount;
        private int _articleId;
        private DateTime _fromUtc;
        private Guid _id;
        private Order _order;
        private string _text;
        private DateTime _toUtc;
        private decimal _unitPrice;

        protected OrderItem()
        {
        }

        public OrderItem(Order order, Article article, DateTime fromUtc, DateTime toUtc, int amount)
        {
            _id = Guid.NewGuid();
            _order = order;
            _articleId = article.Id;
            _unitPrice = article.Price;
            _text = article.Caption;
            _fromUtc = fromUtc;
            _toUtc = toUtc;
            _amount = amount;
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
            set
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

        public virtual decimal LineTotal
        {
            get { return new PerDayWithPerSevenDaysPricePricingStrategy().Calculate(FromUtc.ToLocalTime(), ToUtc.ToLocalTime(), Amount, UnitPrice).Price; }
        }

        public virtual Period PeriodUtc { get { return new Period(FromUtc, ToUtc);} }

        public virtual void ChangeQuantity(int amount)
        {
            Amount = amount;
        }

        public virtual void ChangePeriod(DateTime fromUtc, DateTime toUtc)
        {
            FromUtc = fromUtc;
            ToUtc = toUtc;
        }

        public virtual void Delete()
        {
            _order = null;
        }
    }
}