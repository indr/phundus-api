using System;
using Iesi.Collections.Generic;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Entities
{
    public class Cart : Entity
    {
        private User _customer;
        private ISet<CartItem> _items = new HashedSet<CartItem>();

        public Cart()
        {
        }

        public Cart(User customer)
        {
            _customer = customer;
        }

        public virtual User Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public virtual ISet<CartItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        protected virtual void AddItem(CartItem item)
        {
            Items.Add(item);
            item.Cart = this;
        }

        public virtual void AddItem(int articleId, int quantity, DateTime @from, DateTime to)
        {
            var item = new CartItem();
            item.Article = IoC.Resolve<IArticleRepository>().Get(articleId);
            item.Quantity = quantity;
            item.From = from;
            item.To = to;
            
            AddItem(item);
        }
    }

    public class CartItem : Entity
    {
        public virtual Cart Cart { get; set; }

        public virtual int Quantity { get; set; }
        public virtual DateTime From { get; set; }
        public virtual DateTime To { get; set; }

        public virtual Article Article { get; set; }

        public virtual double LineTotal
        {
            get { return Quantity*Article.Price; }
        }
    }
}