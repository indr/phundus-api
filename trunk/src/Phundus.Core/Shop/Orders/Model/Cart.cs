namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ddd;
    using IdentityAndAccess.Users.Model;
    using Iesi.Collections.Generic;
    using Inventory.Articles.Repositories;
    using Inventory.Services;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;
    using Repositories;
    using Services;
    using Shop.Services;

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

        public virtual void AddItem(Article article, int quantity, DateTime @from, DateTime to)
        {
            var item = new CartItem();
            item.Article = article;
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

        public virtual ICollection<Order> PlaceOrders(ILessorService lessorService, IBorrowerService borrowerService, IAvailabilityService availabilityService)
        {
            var result = new List<Order>();
            var orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();

            var lessors = FindLessors(lessorService);
            var borrower = borrowerService.ById(Customer.Id);

            foreach (var lessor in lessors)
            {
                var order = new Order(lessor, borrower);
                var items = from i in Items where i.Article.Owner.OwnerId == lessor.LessorId select i;
                foreach (var item in items)
                    order.AddItem(item.Article, item.Quantity, item.From.ToUniversalTime(),
                        item.To.Date.AddDays(1).AddSeconds(-1).ToUniversalTime(), availabilityService);

                orderRepository.Add(order);
                result.Add(order);
            }
            ServiceLocator.Current.GetInstance<ICartRepository>().Remove(this);
            return result;
        }

        private IEnumerable<Lessor> FindLessors(ILessorService lessorService)
        {
            var lessorIds = (from cartItems in Items select cartItems.Article.Owner.OwnerId).Distinct();
            return lessorIds.Select(lessorService.GetById).ToList();
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