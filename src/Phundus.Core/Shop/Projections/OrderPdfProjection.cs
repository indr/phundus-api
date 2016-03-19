namespace Phundus.Shop.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Projecting;
    using Model.Orders;
    using Orders.Model;

    public class OrderPdfProjection : ProjectionBase,
        ISubscribeTo<OrderPlaced>,
        ISubscribeTo<OrderApproved>,
        ISubscribeTo<OrderClosed>,
        ISubscribeTo<OrderRejected>
    {
        private readonly IOrderPdfStore _orderPdfStore;

        public OrderPdfProjection(IOrderPdfStore orderPdfStore)
        {
            _orderPdfStore = orderPdfStore;
        }

        public void Handle(OrderApproved e)
        {
            Generate(e.OrderId);
        }

        public void Handle(OrderClosed e)
        {
            Generate(e.OrderId);
        }

        public void Handle(OrderPlaced e)
        {
            Generate(e.OrderId);
        }

        public void Handle(OrderRejected e)
        {
            Generate(e.OrderId);
        }

        private void Generate(Guid orderId)
        {
            _orderPdfStore.Get(new OrderId(orderId));
        }
    }
}