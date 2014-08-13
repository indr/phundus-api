namespace Phundus.Core.Shop.Orders
{
    using System.IO;
    using Castle.Transactions;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Queries;
    using IdentityAndAccess.Users.Repositories;
    using Mails;
    using Microsoft.Practices.ServiceLocation;
    using Queries;
    using Repositories;
    using Services;

    public class OrderService : AppServiceBase, IOrderService
    {
        public IUserRepository Users { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Reject(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);

            var userId = Users.FindByEmail(Identity.Name).Id;

            order.Reject(userId);
            repo.Update(order);

            new OrderRejectedMail()
                .For(order, OrganizationRepository.ById(order.OrganizationId))
                .Send(order.Borrower.EmailAddress);
        }

        public void Confirm(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);

            var userId = Users.FindByEmail(Identity.Name).Id;

            order.Approve(userId);
            repo.Update(order);

            new OrderApprovedMail()
                .For(order, OrganizationRepository.ById(order.OrganizationId))
                .Send(order.Borrower.EmailAddress);
        }

        [Transaction]
        public Stream GetPdf(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);
            return PdfGenerator.GeneratePdf(order, OrganizationRepository.ById(order.OrganizationId));
        }

        public Stream GetPdf(int organizationId, int orderId, int currentUserId)
        {
            MemberInRole.ActiveChief(organizationId, currentUserId);

            return GetPdf(orderId);
        }
    }
}