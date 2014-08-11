namespace Phundus.Core.Shop.Orders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Castle.Transactions;
    using IdentityAndAccess.Organizations.Repositories;
    using IdentityAndAccess.Users.Repositories;
    using Mails;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Queries;
    using Repositories;

    public class OrderService : AppServiceBase, IOrderService
    {
        public IUserRepository Users { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        public void Reject(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);

            var user = Users.FindByEmail(Identity.Name);
            order.Reject(user);
            repo.Update(order);

            new OrderRejectedMail()
                .For(order, OrganizationRepository.ById(order.OrganizationId))
                .Send(order.Reserver);
        }

        public void Confirm(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);

            var user = Users.FindByEmail(Identity.Name);
            order.Approve(user);
            repo.Update(order);

            new OrderApprovedMail()
                .For(order, OrganizationRepository.ById(order.OrganizationId))
                .Send(order.Reserver);
        }

        [Transaction]
        public Stream GetPdf(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);
            return order.GeneratePdf(OrganizationRepository.ById(order.OrganizationId));
        }        
    }
}