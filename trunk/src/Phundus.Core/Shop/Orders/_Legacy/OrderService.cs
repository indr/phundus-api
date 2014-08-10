namespace Phundus.Core.Shop.Orders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Castle.Transactions;
    using IdentityAndAccess.Users.Repositories;
    using Mails;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Queries;
    using Repositories;

    public class OrderService : AppServiceBase, IOrderService
    {
        public IUserRepository Users { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        public virtual OrderDto GetOrder(int id)
        {
            return OrderQueries.FindById(id);
        }

        public virtual IEnumerable<OrderDto> GetMyOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            return OrderQueries.FindByUserId(user.Id);
        }

        public IEnumerable<OrderDto> GetOrders(OrderStatus status, int organizationId)
        {
            return OrderQueries.FindByOrganizationId(organizationId, 0, status);            
        }

        public OrderDto Reject(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);

            var user = Users.FindByEmail(Identity.Name);
            order.Reject(user);
            repo.Update(order);

            new OrderRejectedMail()
                .For(order)
                .Send(order.Reserver);


            return GetOrder(id);
        }

        public OrderDto Confirm(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);

            var user = Users.FindByEmail(Identity.Name);
            order.Approve(user);
            repo.Update(order);

            new OrderApprovedMail()
                .For(order)
                .Send(order.Reserver);


            return GetOrder(id);
        }

        [Transaction]
        public Stream GetPdf(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.ById(id);
            return order.GeneratePdf();
        }        
    }
}