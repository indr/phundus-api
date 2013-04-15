namespace phiNdus.fundus.Web.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Castle.Transactions;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Mails;
    using phiNdus.fundus.Domain.Repositories;
    using phiNdus.fundus.Web.Business.Assembler;
    using phiNdus.fundus.Web.Business.Dto;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class OrderService : BaseService, IOrderService
    {
        public IUserRepository Users { get; set; }

        #region IOrderService Members

        public virtual OrderDto GetOrder(int id)
        {
            using (UnitOfWork.Start())
            {
                var order = ServiceLocator.Current.GetInstance<IOrderRepository>().Get(id);
                if (order == null)
                    return null;
                return new OrderDtoAssembler().CreateDto(order);
            }
        }

        public virtual IList<OrderDto> GetPendingOrders()
        {
            using (UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                           .FindPending(user.SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetApprovedOrders()
        {
            using (UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                           .FindApproved(user.SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetRejectedOrders()
        {
            using (UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                           .FindRejected(user.SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetMyOrders()
        {
            using (UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var orders = ServiceLocator.Current.GetInstance<IOrderRepository>().FindMy(user.Id);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
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
            using (var uow = UnitOfWork.Start())
            {
                var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
                var order = repo.Get(id);

                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                order.Reject(user);
                repo.Update(order);

                new OrderRejectedMail()
                    .For(order)
                    .Send(order.Reserver);

                uow.TransactionalFlush();
            }

            return GetOrder(id);
        }

        public OrderDto Confirm(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
                var order = repo.Get(id);

                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                order.Approve(user);
                repo.Update(order);

                new OrderApprovedMail()
                    .For(order)
                    .Send(order.Reserver);

                uow.TransactionalFlush();
            }
            return GetOrder(id);
        }

        [Transaction]
        public Stream GetPdf(int id)
        {
            var repo = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var order = repo.Get(id);
            return order.GeneratePdf();
        }

        #endregion

        private IList<OrderDto> GetClosedOrders()
        {
            using (UnitOfWork.Start())
            {
                var user = Users.FindByEmail(HttpContext.Current.User.Identity.Name);
                var orders = ServiceLocator.Current.GetInstance<IOrderRepository>()
                                           .FindClosed(user.SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }
    }
}