﻿namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using Ddd;
    using Inventory.Model;

    public class OrderItem : EntityBase
    {
        public virtual Order Order { get; set; }

        private int _amount;

        public virtual int Amount
        {
            get { return _amount; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "Die Menge darf nicht kleiner als Eins sein.");
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
            get { return Article.Price * Amount; }
        }
    }
}