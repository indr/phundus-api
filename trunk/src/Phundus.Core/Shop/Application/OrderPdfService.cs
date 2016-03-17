namespace Phundus.Shop.Application
{
    using System.IO;
    using Common.Domain.Model;
    using Model;
    using Model.Orders;

    public interface IOrderPdfService
    {
        OrderPdfData GetOrderPdf(UserId currentUserId, OrderId orderId);
    }

    public class OrderPdfService : IOrderPdfService
    {
        private readonly IOrderPdfStore _orderPdfStore;
        private readonly IOrderRepository _orderRepository;

        public OrderPdfService(IOrderPdfStore orderPdfStore, IOrderRepository orderRepository)
        {
            _orderPdfStore = orderPdfStore;
            _orderRepository = orderRepository;
        }

        public OrderPdfData GetOrderPdf(UserId currentUserId, OrderId orderId)
        {
            // TODO: Authorization
            // TODO: Use order query service
            var order = _orderRepository.GetById(orderId);
            return new OrderPdfData
            {
                OrderShortId = order.OrderShortId.Id,
                Stream = _orderPdfStore.Get(orderId, order.MutatedVersion - 1)
            };
        }
    }

    public class OrderPdfData
    {
        public int OrderShortId { get; set; }
        public Stream Stream { get; set; }
    }
}