namespace Phundus.Shop.Model
{
    using System;
    using Common.Domain.Model;

    public class OrderLine
    {
        private ArticleId _articleId;
        private ArticleShortId _articleShortId;
        private decimal _lineTotal;
        private OrderLineId _orderLineId;
        private Period _period;
        private int _quantity;
        private string _text;
        private decimal _unitPricePerWeekPerWeek;

        public OrderLine(OrderLineId orderLineId, ArticleId articleId, ArticleShortId articleShortId, StoreId storeId,
            string text, Period period, int quantity, decimal unitPricePerWeekPerWeek, decimal lineTotal)
        {
            if (orderLineId == null) throw new ArgumentNullException("orderLineId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (text == null) throw new ArgumentNullException("text");
            if (period == null) throw new ArgumentNullException("period");

            _orderLineId = orderLineId;
            _articleId = articleId;
            _articleShortId = articleShortId;
            StoreId = storeId;
            _text = text;
            _period = period;
            _quantity = quantity;
            _unitPricePerWeekPerWeek = unitPricePerWeekPerWeek;
            _lineTotal = lineTotal;
        }

        public virtual OrderLineId LineId
        {
            get { return _orderLineId; }
            protected set { _orderLineId = value; }
        }

        public virtual int Quantity
        {
            get { return _quantity; }
            private set { _quantity = value; }
        }

        public virtual Period Period
        {
            get { return _period; }
            private set { _period = value; }
        }

        public virtual ArticleShortId ArticleShortId
        {
            get { return _articleShortId; }
            protected set { _articleShortId = value; }
        }

        public virtual ArticleId ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

        public StoreId StoreId { get; private set; }

        public virtual string Text
        {
            get { return _text; }
            protected set { _text = value; }
        }

        public virtual decimal UnitPricePerWeek
        {
            get { return _unitPricePerWeekPerWeek; }
            protected set { _unitPricePerWeekPerWeek = value; }
        }

        public virtual decimal LineTotal
        {
            get { return _lineTotal; }
            protected set { _lineTotal = value; }
        }

        public virtual void ChangeQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public virtual void ChangePeriod(Period period)
        {
            Period = period;
        }

        public virtual void ChangeLineTotal(decimal lineTotal)
        {
            LineTotal = lineTotal;
        }
    }
}