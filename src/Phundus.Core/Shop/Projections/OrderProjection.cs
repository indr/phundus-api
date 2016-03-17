namespace Phundus.Shop.Projections
{
    using System;
    using System.Linq;
    using Application;
    using Common.Eventing;
    using Common.Projecting;
    using Orders.Model;

    public class OrderProjection : ProjectionBase<OrderData>,
        ISubscribeTo<OrderCreated>,
        ISubscribeTo<OrderApproved>,
        ISubscribeTo<OrderClosed>,
        ISubscribeTo<OrderRejected>,
        ISubscribeTo<OrderItemAdded>,
        ISubscribeTo<OrderItemPeriodChanged>,
        ISubscribeTo<OrderItemQuantityChanged>,
        ISubscribeTo<OrderItemRemoved>,
        ISubscribeTo<OrderItemTotalChanged>
    {
        public void Handle(OrderApproved e)
        {
            Update(e.OrderId, order =>
                order.Status = OrderData.OrderStatus.Approved);
        }

        public void Handle(OrderClosed e)
        {
            Update(e.OrderId, order =>
                order.Status = OrderData.OrderStatus.Closed);
        }

        public void Handle(OrderCreated e)
        {
            Insert(order =>
            {
                order.OrderId = e.OrderId;
                order.OrderShortId = e.OrderShortId;
                order.CreatedAtUtc = e.OccuredOnUtc;
                order.ModifiedAtUtc = e.OccuredOnUtc;
                order.Status = OrderData.OrderStatus.Pending;
                order.OrderTotal = 0.0m;

                order.LesseeId = e.Lessee.LesseeId.Id;
                order.LesseeFirstName = e.Lessee.FirstName;
                order.LesseeLastName = e.Lessee.LastName;
                order.LesseeStreet = e.Lessee.Street;
                order.LesseePostcode = e.Lessee.Postcode;
                order.LesseeCity = e.Lessee.City;
                order.LesseeEmailAddress = e.Lessee.EmailAddress;
                order.LesseePhoneNumber = e.Lessee.PhoneNumber;

                order.LessorId = e.Lessor.LessorId.Id;
                order.LessorName = e.Lessor.Name;
                order.LessorStreet = null;
                order.LessorPostcode = null;
                order.LessorCity = null;
                order.LessorEmailAddress = null;
                order.LessorPhoneNumber = null;

                if (e.Lines != null)
                    order.Lines = e.Lines.Select(s => CreateOrderLineData(order, s)).ToList();
            });
        }

        public void Handle(OrderItemAdded e)
        {
            Update(e.OrderId, order =>
                order.Lines.Add(CreateOrderLineData(order, e.OrderLine)));
        }

        public void Handle(OrderItemPeriodChanged e)
        {
            UpdateOrderLine(e.OrderId, e.OrderItemId, line =>
            {
                line.FromUtc = e.NewPeriod.FromUtc;
                line.ToUtc = e.NewPeriod.ToUtc;
            });
        }

        public void Handle(OrderItemQuantityChanged e)
        {
            UpdateOrderLine(e.OrderId, e.OrderItemId, line => { line.Quantity = e.NewQuantity; });
        }

        public void Handle(OrderItemRemoved e)
        {
            Update(e.OrderId, order =>
            {
                var item = order.Lines.Single(p => p.LineId == e.OrderLine.LineId);
                order.Lines.Remove(item);
            });
        }

        public void Handle(OrderItemTotalChanged e)
        {
            UpdateOrderLine(e.OrderId, e.OrderItemId, line => { line.LineTotal = e.NewItemTotal; });
        }

        public void Handle(OrderRejected e)
        {
            Update(e.OrderId, order =>
                order.Status = OrderData.OrderStatus.Rejected);
        }

        private static OrderLineData CreateOrderLineData(OrderData order, OrderEventLine s)
        {
            return new OrderLineData
            {
                LineId = s.LineId,
                Order = order,
                ArticleId = s.ArticleId,
                ArticleShortId = s.ArticleShortId,
                Text = s.Text,
                FromUtc = s.FromUtc,
                ToUtc = s.ToUtc,
                Quantity = s.Quantity,
                UnitPricePerWeek = s.UnitPricePerWeek,
                LineTotal = s.LineTotal
            };
        }

        private void UpdateOrderLine(Guid orderId, Guid lineId, Action<OrderLineData> action)
        {
            Update(orderId, a =>
            {
                var line = a.Lines.Single(p => p.LineId == lineId);
                action(line);
            });
        }
    }
}