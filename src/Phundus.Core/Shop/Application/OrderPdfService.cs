namespace Phundus.Shop.Application
{
    using System;
    using System.IO;
    using Common.Domain.Model;
    using Model.Orders;

    public interface IOrderPdfService
    {
        OrderPdfData GetOrderPdf(InitiatorId initiatorId, OrderId orderId);
    }

    public class OrderPdfService : IOrderPdfService
    {
        private readonly IOrderPdfStore _orderPdfStore;
        private readonly IOrderQueryService _orderQueryService;

        public OrderPdfService(IOrderPdfStore orderPdfStore, IOrderQueryService orderQueryService)
        {
            _orderPdfStore = orderPdfStore;
            _orderQueryService = orderQueryService;
        }

        public OrderPdfData GetOrderPdf(InitiatorId initiatorId, OrderId orderId)
        {
            // Checks authorization with initiatorId
            var order = _orderQueryService.GetById(initiatorId, orderId);

            var orderPdf = _orderPdfStore.Get(new OrderId(order.OrderId));
            return new OrderPdfData
            {
                OrderId = orderPdf.OrderId.Id,
                OrderShortId = orderPdf.OrderShortId.Id,
                Stream = orderPdf.GetStreamCopy()
            };
        }
    }

    public class OrderPdfData
    {
        public Guid OrderId { get; set; }
        public int OrderShortId { get; set; }
        public Stream Stream { get; set; }
    }
}