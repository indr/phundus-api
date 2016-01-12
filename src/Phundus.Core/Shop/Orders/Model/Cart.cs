namespace Phundus.Core.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Inventory.Services;
    using Repositories;
    using Shop.Services;

    public class Cart : EntityBase
    {
        private int _customerId;
        private Iesi.Collections.Generic.ISet<CartItem> _items = new HashedSet<CartItem>();

        public Cart(UserId userId)
        {
            _customerId = userId.Id;
        }

        protected Cart()
        {
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
            get { return _customerId; }
            protected set { _customerId = value; }
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

        public virtual ICollection<Order> PlaceOrders(ICartRepository cartRepository, IOrderRepository orderRepository,
            ILessorService lessorService, ILesseeService lesseeService, IAvailabilityService availabilityService)
        {
            var result = new List<Order>();
            var lessors = FindLessors(lessorService);
            var borrower = lesseeService.GetById(CustomerId);

            foreach (var lessor in lessors)
            {
                var order = new Order(lessor, borrower);
                var items = from i in Items where i.Article.Owner.OwnerId.Id == lessor.LessorId.Id select i;
                foreach (var item in items)
                    order.AddItem(item.Article, item.Quantity, item.From.ToUniversalTime(),
                        item.To.Date.AddDays(1).AddSeconds(-1).ToUniversalTime(), availabilityService);

                orderRepository.Add(order);
                result.Add(order);
            }
            cartRepository.Remove(this);
            return result;
        }

        private IEnumerable<Lessor> FindLessors(ILessorService lessorService)
        {
            var lessorIds = (from cartItems in Items select cartItems.Article.Owner.OwnerId.Id).Distinct();
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