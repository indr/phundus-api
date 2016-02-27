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

        public decimal GetOrderLinesSum()
        {
            return _items.Count == 0 ? 0.0m : _items.Sum(s => s.LineTotal);
        }

        public IList<OrderLine> Lines
        {
            get { return new ReadOnlyCollection<OrderLine>(_items);}
        } 

        public void When(OrderItemAdded e)
        {
            var item = new OrderLine(new OrderLineId(e.OrderItem.ItemId), new ArticleId(e.OrderItem.ArticleId),
                new ArticleShortId(e.OrderItem.ArticleShortId),
                e.OrderItem.Text, e.OrderItem.Period, e.OrderItem.Quantity, e.OrderItem.UnitPricePerWeek,
                e.OrderItem.ItemTotal);
            _items.Add(item);
        }

        public void When(OrderItemRemoved e)
        {
            var item = GetOrderLine(e.OrderItem.ItemId);
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