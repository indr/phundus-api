namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using Inventory.Articles.Model;

    public class OrderItem
    {
        private int _amount;
        private Guid _id;
        private Order _order;
        private DateTime _fromUtc;
        private DateTime _toUtc;
        private Article _article;

        protected OrderItem()
        {
        }

        public OrderItem(Order order, Article article, DateTime fromUtc, DateTime toUtc, int amount)
        {
            _id = Guid.NewGuid();
            _order = order;
            _article = article;
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

        public virtual int ArticleId { get { return Article.Id; } }

        public virtual Article Article
        {
            get { return _article; }
            protected set { _article = value; }
        }

        public virtual string Text
        {
            get { return Article.Caption; }
        }

        public virtual decimal UnitPrice
        {
            get { return Article.Price; }
        }

        public virtual decimal LineTotal
        {
            get { return UnitPrice * Amount; }
        }

        public virtual void ChangeAmount(int amount)
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