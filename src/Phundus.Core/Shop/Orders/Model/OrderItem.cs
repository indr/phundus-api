namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using Inventory.Model;

    public class OrderItem
    {
        private int _amount;
        private Guid _id;
        private Order _order;

        protected OrderItem()
        {
        }

        public OrderItem(Order order)
        {
            _id = Guid.NewGuid();
            _order = order;
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

        public virtual DateTime From { get; set; }

        public virtual DateTime To { get; set; }

        public virtual Article Article { get; set; }

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

        public virtual void ChangePeriod(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public virtual void Delete()
        {
            _order = null;
        }
    }
}