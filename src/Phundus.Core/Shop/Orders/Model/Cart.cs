namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ddd;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Users.Model;
    using Iesi.Collections.Generic;
    using InventoryCtx.Model;
    using InventoryCtx.Repositories;
    using InventoryCtx.Services;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;
    using Repositories;

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

        public virtual int CustomerId
        {
            get { return Customer.Id; }
        }

        public virtual void AddItem(int articleId, int quantity, DateTime @from, DateTime to)
        {
            var item = new CartItem();
            item.Article = ServiceLocator.Current.GetInstance<IArticleRepository>().ById(articleId);
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

        public virtual void CalculateAvailability(ISession session)
        {
            foreach (var each in Items)
            {
                var checker = new AvailabilityChecker(each.Article, session);
                each.IsAvailable = checker.Check(each.From, each.To, each.Quantity);
            }
        }

        public virtual Order PlaceOrder(ISession session)
        {
            var result = new Order();
            result.Reserver = Customer;
            foreach (var each in Items)
                result.AddItem(each.Article.Id, each.Quantity, each.From, each.To, session);

            ServiceLocator.Current.GetInstance<IOrderRepository>().Add(result);
            ServiceLocator.Current.GetInstance<ICartRepository>().Remove(this);
            return result;
        }

        public virtual ICollection<Order> PlaceOrders(ISession session)
        {
            var result = new List<Order>();
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var organizationRepository = ServiceLocator.Current.GetInstance<IOrganizationRepository>();


            var organizationIds = (from i in Items select i.Article.OrganizationId).Distinct();
            var organizations = new List<Organization>();
            foreach (var each in organizationIds)
                organizations.Add(organizationRepository.ById(each));

            foreach (var organization in organizations)
            {
                var order = new Order();
                order.Organization = organization;
                order.Reserver = Customer;
                var items = from i in Items where i.Article.OrganizationId == organization.Id select i;
                foreach (var item in items)
                    order.AddItem(item.Article.Id, item.Quantity, item.From, item.To, session);

                orders.Add(order);
                result.Add(order);
            }
            ServiceLocator.Current.GetInstance<ICartRepository>().Remove(this);
            return result;
        }

        public virtual void Clear()
        {
            CartItem item;
            while (null != (item = Items.FirstOrDefault()))
            {
                RemoveItem(item);
            }
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
            get { return Article.Name; }
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