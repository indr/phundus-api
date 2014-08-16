namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using Ddd;
    using Inventory.Articles.Model;

    public class CartItem : EntityBase
    {
        public virtual Cart Cart { get; set; }

        public virtual Article Article { get; set; }
        public virtual int Quantity { get; set; }
        public virtual DateTime From { get; set; }
        public virtual DateTime To { get; set; }

        public virtual string LineText
        {
            get { return Article.Caption; }
        }

        public virtual decimal UnitPrice
        {
            get { return Article.Price; }
        }

        public virtual decimal LineTotal
        {
            get { return Quantity*UnitPrice; }
        }

        public virtual bool IsAvailable { get; set; }
    }
}