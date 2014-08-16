namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.Services;
    using Ddd;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Users.Model;
    using Iesi.Collections.Generic;
    using Inventory.Articles.Repositories;
    using Inventory._Legacy;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;
    using Repositories;
    using Services;

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

        public virtual ICollection<Order> PlaceOrders(ISession session, IBorrowerService borrowerService)
        {
            var result = new List<Order>();
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var organizationRepository = ServiceLocator.Current.GetInstance<IOrganizationService>();


            var organizationIds = (from i in Items select i.Article.OrganizationId).Distinct();
            var organizations = new List<Organization>();
            foreach (var each in organizationIds)
                organizations.Add(organizationRepository.ById(each));

            foreach (var organization in organizations)
            {
                var order = new Order(organization, borrowerService.ById(Customer.Id));
                var items = from i in Items where i.Article.OrganizationId == organization.Id select i;
                foreach (var item in items)
                    order.AddItem(item.Article.Id, item.Quantity, item.From.ToUniversalTime(), item.To.Date.AddDays(1).AddSeconds(-1).ToUniversalTime(), session);

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
}