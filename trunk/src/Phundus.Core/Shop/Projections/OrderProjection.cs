namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Orders.Model;

    public class OrderProjection : ProjectionBase<OrderData>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(OrderCreated e)
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

        private void Process(OrderApproved e)
        {
            Update(e.OrderId, order =>
                order.Status = OrderData.OrderStatus.Approved);
        }

        private void Process(OrderClosed e)
        {
            Update(e.OrderId, order =>
                order.Status = OrderData.OrderStatus.Closed);
        }

        private void Process(OrderRejected e)
        {
            Update(e.OrderId, order =>
                order.Status = OrderData.OrderStatus.Rejected);
        }

        private void Process(OrderItemAdded e)
        {
            Update(e.OrderId, order =>
                order.Lines.Add(CreateOrderLineData(order, e.OrderLine)));
        }

        private void Process(OrderItemRemoved e)
        {
            Update(e.OrderId, order =>
            {
                var item = order.Lines.Single(p => p.LineId == e.OrderLine.ItemId);
                order.Lines.Remove(item);
            });
        }

        private void Process(OrderItemPeriodChanged e)
        {
            UpdateOrderLine(e.OrderId, e.OrderItemId, line =>
            {
                line.FromUtc = e.NewPeriod.FromUtc;
                line.ToUtc = e.NewPeriod.ToUtc;
            });
        }

        private void Process(OrderItemQuantityChanged e)
        {
            UpdateOrderLine(e.OrderId, e.OrderItemId, line => { line.Quantity = e.NewQuantity; });
        }

        private void Process(OrderItemTotalChanged e)
        {
            UpdateOrderLine(e.OrderId, e.OrderItemId, line => { line.LineTotal = e.NewItemTotal; });
        }

        private static OrderLineData CreateOrderLineData(OrderData order, OrderEventLine s)
        {
            return new OrderLineData
            {
                LineId = s.ItemId,
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

    public class OrderData
    {
        private ICollection<OrderLineData> _lines = new Collection<OrderLineData>();

        public enum OrderStatus
        {
            Pending = 1,
            Approved,
            Rejected,
            Closed
        }

        public virtual Guid OrderId { get; set; }
        public virtual int OrderShortId { get; set; }
        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual DateTime ModifiedAtUtc { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual decimal OrderTotal { get; set; }

        public virtual Guid LessorId { get; set; }
        public virtual string LessorName { get; set; }
        public virtual string LessorStreet { get; set; }
        public virtual string LessorPostcode { get; set; }
        public virtual string LessorCity { get; set; }
        public virtual string LessorEmailAddress { get; set; }
        public virtual string LessorPhoneNumber { get; set; }

        public virtual Guid LesseeId { get; set; }
        public virtual string LesseeFirstName { get; set; }
        public virtual string LesseeLastName { get; set; }
        public virtual string LesseeStreet { get; set; }
        public virtual string LesseePostcode { get; set; }
        public virtual string LesseeCity { get; set; }
        public virtual string LesseeEmailAddress { get; set; }
        public virtual string LesseePhoneNumber { get; set; }

        public virtual ICollection<OrderLineData> Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }
    }

    public class OrderLineData
    {
        public virtual Guid DataId { get; set; }

        public virtual Guid LineId { get; set; }
        public virtual OrderData Order { get; set; }

        public virtual Guid ArticleId { get; set; }
        public virtual int ArticleShortId { get; set; }

        public virtual string Text { get; set; }
        public virtual DateTime FromUtc { get; set; }
        public virtual DateTime ToUtc { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPricePerWeek { get; set; }
        public virtual decimal LineTotal { get; set; }
        public virtual bool IsAvailable { get; set; }
    }
}