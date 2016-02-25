namespace Phundus.Shop.Model.Pdf
{
    using System.IO;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Orders.Model;
    using Orders.Repositories;
    using Orders.Services;
    using Projections;

    public interface IPdfStore
    {
        Stream GetOrderPdf(int orderId, UserId currentUserId);
    }

    public class PdfStore : IPdfStore
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public Stream GetOrderPdf(int orderId, UserId currentUserId)
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