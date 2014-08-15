namespace Phundus.Core.Shop.Orders
{
    using System.IO;
    using Castle.Transactions;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Queries;
    using Queries;
    using Repositories;
    using Services;

    public class OrderService : AppServiceBase, IOrderService
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public Stream GetPdf(int id)
        {
            var order = OrderRepository.ById(id);
            return OrderPdfGeneratorService.GeneratePdf(order, OrganizationRepository.ById(order.OrganizationId));
        }

        public Stream GetPdf(int organizationId, int orderId, int currentUserId)
        {
            MemberInRole.ActiveChief(organizationId, currentUserId);

            return GetPdf(orderId);
        }
    }
}