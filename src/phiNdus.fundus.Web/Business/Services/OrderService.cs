namespace phiNdus.fundus.Web.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Castle.Transactions;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Web.Business.Assembler;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core.Entities;
    using Phundus.Core.IdentityAndAccessCtx.Repositories;
    using Phundus.Core.Mails;
    using Phundus.Core.Repositories;

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

        public virtual IList<OrderDto> GetPendingOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                       .FindPending(user.SelectedOrganization);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public virtual IList<OrderDto> GetApprovedOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                       .FindApproved(user.SelectedOrganization);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public virtual IList<OrderDto> GetRejectedOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                       .FindRejected(user.SelectedOrganization);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public virtual IList<OrderDto> GetMyOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>().FindMy(user.Id);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public IList<OrderDto> GetOrders(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Pending:
                    return GetPendingOrders();
                case OrderStatus.Approved:
                    return GetApprovedOrders();
                case OrderStatus.Rejected:
                    return GetRejectedOrders();
                case OrderStatus.Closed:
                    return GetClosedOrders();
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

        private IList<OrderDto> GetClosedOrders()
        {
            var user = Users.FindByEmail(Identity.Name);
            var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                       .FindClosed(user.SelectedOrganization);
            return new OrderDtoAssembler().CreateDtos(orders);
        }
    }
}