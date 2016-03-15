namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Domain.Model;
    using Orders.Model;

    public class OrderLines
    {
        private readonly IList<OrderLine> _items = new List<OrderLine>();

        public OrderLines()
        {
        }

        public OrderLines(IEnumerable<CartItem> cartItems)
        {
            if (cartItems == null)
                return;

            foreach (var each in cartItems)
            {
                var item = new OrderLine(new OrderLineId(), each.ArticleId, each.ArticleShortId, each.StoreId, each.LineText,
                    each.Period, each.Quantity, each.UnitPrice, each.ItemTotal);
                _items.Add(item);
            }
        }

        public OrderLines(IEnumerable<OrderEventLine> orderEventLines)
        {
            if (orderEventLines == null)
                return;

            foreach (var each in orderEventLines)
            {
                var item = new OrderLine(new OrderLineId(each.LineId), new ArticleId(each.ArticleId),
                    new ArticleShortId(each.ArticleShortId), new StoreId(each.StoreId), each.Text, each.Period, each.Quantity,
                    each.UnitPricePerWeek, each.LineTotal);
                _items.Add(item);
            }
        }

        public IList<OrderLine> Lines
        {
            get { return new ReadOnlyCollection<OrderLine>(_items); }
        }

        public decimal GetOrderLinesSum()
        {
            return _items.Count == 0 ? 0.0m : _items.Sum(s => s.LineTotal);
        }

        public void When(OrderItemAdded e)
        {
            var item = new OrderLine(new OrderLineId(e.OrderLine.LineId), new ArticleId(e.OrderLine.ArticleId),
                new ArticleShortId(e.OrderLine.ArticleShortId), new StoreId(e.OrderLine.StoreId), 
                e.OrderLine.Text, e.OrderLine.Period, e.OrderLine.Quantity, e.OrderLine.UnitPricePerWeek,
                e.OrderLine.LineTotal);
            _items.Add(item);
        }

        public void When(OrderItemRemoved e)
        {
            var item = GetOrderLine(e.OrderLine.LineId);
            _items.Remove(item);
        }

        public void When(OrderItemQuantityChanged e)
        {
            var item = GetOrderLine(e.OrderItemId);
            item.ChangeQuantity(e.NewQuantity);
        }

        public void When(OrderItemPeriodChanged e)
        {
            var item = GetOrderLine(e.OrderItemId);
            item.ChangePeriod(e.NewPeriod);
        }

        public void When(OrderItemTotalChanged e)
        {
            var item = GetOrderLine(e.OrderItemId);
            item.ChangeLineTotal(e.NewItemTotal);
        }

        public OrderLine GetOrderLine(Guid orderLineId)
        {
            var item = Find(orderLineId);
            if (item == null)
                throw new InvalidOperationException(String.Format("Could not find order line {0}", orderLineId));
            return item;
        }

        private OrderLine Find(Guid orderLineId)
        {
            return _items.FirstOrDefault(p => p.LineId.Id == orderLineId);
        }
    }
}