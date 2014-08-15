namespace Phundus.Core.Shop.Orders
{
    using System.IO;
    using Castle.Transactions;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Queries;
    using Queries;
    using Repositories;
    using Services;

    public class PdfStore : IPdfStore
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public Stream GetOrderPdf(int orderId)
        {
            var order = OrderRepository.ById(orderId);
            return OrderPdfGeneratorService.GeneratePdf(order);
        }

        public Stream GetOrderPdf(int organizationId, int orderId, int currentUserId)
        {
            MemberInRole.ActiveChief(organizationId, currentUserId);

            return GetOrderPdf(orderId);
        }
    }
}