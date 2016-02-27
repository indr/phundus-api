namespace Phundus.Shop.Model.Pdf
{
    using System.IO;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Orders.Model;
    using Orders.Repositories;
    using Orders.Services;

    public interface IPdfStore
    {
        Stream GetOrderPdf(OrderId orderId, UserId currentUserId);
    }

    public class PdfStore : IPdfStore
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public Stream GetOrderPdf(OrderId orderId, UserId currentUserId)
        {
            var order = OrderRepository.GetById(orderId);
            return GetPdf(order);
        }

        private Stream GetPdf(Order order)
        {
            return OrderPdfGeneratorService.GeneratePdf(order);
        }
    }
}