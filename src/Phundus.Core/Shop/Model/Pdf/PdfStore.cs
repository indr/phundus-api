namespace Phundus.Shop.Model.Pdf
{
    using System.IO;
    using Common.Domain.Model;

    public interface IPdfStore
    {
        Stream GetOrderPdf(OrderId orderId, UserId currentUserId);
    }

    public class PdfStore : IPdfStore
    {
        private readonly IOrderPdfGenerator _orderPdfGenerator;
        private readonly IOrderRepository _orderRepository;

        public PdfStore(IOrderRepository orderRepository, IOrderPdfGenerator orderPdfGenerator)
        {
            _orderRepository = orderRepository;
            _orderPdfGenerator = orderPdfGenerator;
        }

        public Stream GetOrderPdf(OrderId orderId, UserId currentUserId)
        {
            var order = _orderRepository.GetById(orderId);
            return GetPdf(order);
        }

        private Stream GetPdf(Order order)
        {
            return _orderPdfGenerator.GeneratePdf(order);
        }
    }
}