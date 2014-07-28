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

    public class OrderService : BaseService, IOrderService
    {
        public IUserRepository Users { get; set; }

        #region IOrderService Members

        public virtual OrderDto GetOrder(int id)
        {
            var order = ServiceLocator.Current.GetInstance<IOrderRepository>().ById(id);
            if (order == null)
                return null;
            return new OrderDtoAssembler().CreateDto(order);
        }

        public virtual IList<OrderDto> GetPendingOrders(int organizationId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                .FindPending(organizationId);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public virtual IList<OrderDto> GetApprovedOrders(int organizationId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                .FindApproved(organizationId);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public virtual IList<OrderDto> GetRejectedOrders(int organizationId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                .FindRejected(organizationId);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public virtual IList<OrderDto> GetMyOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>().FindMy(user.Id);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public IList<OrderDto> GetOrders(OrderStatus status, int organizationId)
        {
            switch (status)
            {
                case OrderStatus.Pending:
                    return GetPendingOrders(organizationId);
                case OrderStatus.Approved:
                    return GetApprovedOrders(organizationId);
                case OrderStatus.Rejected:
                    return GetRejectedOrders(organizationId);
                case OrderStatus.Closed:
                    return GetClosedOrders(organizationId);
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
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

        #endregion

        private IList<OrderDto> GetClosedOrders(int organizationId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                .FindClosed(organizationId);
            return new OrderDtoAssembler().CreateDtos(orders);
        }
    }
}