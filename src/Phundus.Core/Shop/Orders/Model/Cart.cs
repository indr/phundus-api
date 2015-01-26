namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ddd;
    using Domain.Model.Identity;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Users.Model;
    using Iesi.Collections.Generic;
    using Inventory.Domain.Model.Catalog;
    using Inventory.Services;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;
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
            item.Article = ServiceLocator.Current.GetInstance<IArticleRepository>().GetById(articleId);
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

        public virtual void CalculateAvailability(IAvailabilityService availabilityService)
        {
            foreach (var each in Items)
            {
                each.IsAvailable = availabilityService.IsArticleAvailable(each.ArticleId, each.From, each.To,
                    each.Quantity, Guid.Empty);
            }
        }

        public virtual ICollection<Order> PlaceOrders(IBorrowerService borrowerService, IAvailabilityService availabilityService)
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
                {
                    var fromUtc = item.From.ToUniversalTime();
                    var toUtc = item.To.Date.AddDays(1).AddSeconds(-1).ToUniversalTime();
                    
                    if (!availabilityService.IsArticleAvailable(item.ArticleId, fromUtc, toUtc, item.Quantity, Guid.Empty))
                        throw new ArticleNotAvailableException(Guid.Empty);

                    order.AddItem(new UserId(CustomerId), item.Article, fromUtc, toUtc, item.Quantity);
                    //order.AddItem(item.Article.Id, item.Quantity, fromUtc, toUtc, availabilityService);

                }

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