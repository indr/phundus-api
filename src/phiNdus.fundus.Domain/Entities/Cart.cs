﻿using System;
using System.Collections.Generic;
using System.Linq;
using Iesi.Collections.Generic;
using phiNdus.fundus.Domain.Inventory;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Entities
{
    public class Cart : EntityBase
    {
        private User _customer;
        private Iesi.Collections.Generic.ISet<CartItem> _items = new HashedSet<CartItem>();

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

        public virtual Iesi.Collections.Generic.ISet<CartItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual bool AreItemsAvailable
        {
            get { return Items.Count(p => p.IsAvailable == false) == 0; }
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

        protected virtual void AddItem(CartItem item)
        {
            Items.Add(item);
            item.Cart = this;
        }

        public virtual void RemoveItem(CartItem item)
        {
            item.Cart = null;
            Items.Remove(item);
        }

        public virtual void CalculateAvailability()
        {
            foreach (var each in Items)
            {
                var checker = new AvailabilityChecker(each.Article);
                each.IsAvailable = checker.Check(each.From, each.To, each.Quantity);
            }
        }

        public virtual Order PlaceOrder()
        {
            var result = new Order();
            result.Reserver = Customer;
            foreach (var each in Items)
                result.AddItem(each.Article.Id, each.Quantity, each.From, each.To);

            IoC.Resolve<IOrderRepository>().Save(result);
            IoC.Resolve<ICartRepository>().Delete(this);
            return result;
        }

        public virtual ICollection<Order> PlaceOrders()
        {
            var result = new List<Order>();
            var orders = IoC.Resolve<IOrderRepository>();

            var organizations = (from i in Items select i.Article.Organization).Distinct();

            foreach (var organization in organizations)
            {
                var order = new Order();
                order.Organization = organization;
                order.Reserver = Customer;
                var items = from i in Items where i.Article.Organization.Id == organization.Id select i;
                foreach (var item in items)
                    order.AddItem(item.Article.Id, item.Quantity, item.From, item.To);

                orders.Save(order);
                result.Add(order);
            }
            IoC.Resolve<ICartRepository>().Delete(this);
            return result;
        }
    }

    
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

        public virtual double UnitPrice
        {
            get { return Article.Price; }
        }

        public virtual double LineTotal
        {
            get { return Quantity*UnitPrice; }
        }

        public virtual bool IsAvailable { get; set; }
    }
}