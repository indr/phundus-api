namespace Phundus.Shop.Application
{
    using System.IO;
    using Common.Domain.Model;
    using Model.Orders;

    public interface IOrderPdfService
    {
        Stream GetOrderPdf(OrderId orderId, UserId currentUserId);
    }

    public class OrderPdfService : IOrderPdfService
    {
        private readonly IOrderPdfStore _orderPdfStore;

        public OrderPdfService(IOrderPdfStore orderPdfStore)
        {
            _orderPdfStore = orderPdfStore;
        }

        public Stream GetOrderPdf(OrderId orderId, UserId currentUserId)
        {
            // TODO: Authorization
            return _orderPdfStore.Get(orderId);
        }
    }
}