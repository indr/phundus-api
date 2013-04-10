namespace phiNdus.fundus.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Assembler;
    using Domain.Entities;
    using Domain.Repositories;
    using Dto;
    using Mails;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class OrderService : BaseService, IOrderService
    {
        #region IOrderService Members

        public virtual OrderDto GetOrder(int id)
        {
            using (UnitOfWork.Start())
            {
                var order = GlobalContainer.Resolve<IOrderRepository>().Get(id);
                if (order == null)
                    return null;
                return new OrderDtoAssembler().CreateDto(order);
            }
        }

        public virtual IList<OrderDto> GetPendingOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = GlobalContainer.Resolve<IOrderRepository>().FindPending(SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetApprovedOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = GlobalContainer.Resolve<IOrderRepository>().FindApproved(SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetRejectedOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = GlobalContainer.Resolve<IOrderRepository>().FindRejected(SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetMyOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = GlobalContainer.Resolve<IOrderRepository>().FindMy(SecurityContext.SecuritySession.User.Id);
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
                    return GetPendingOrders();
                case OrderStatus.Rejected:
                    return GetPendingOrders();
                case OrderStatus.Closed:
                    return GetPendingOrders();
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }


        public OrderDto Reject(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var repo = GlobalContainer.Resolve<IOrderRepository>();
                var order = repo.Get(id);

                order.Reject(SecurityContext.SecuritySession.User);
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
                var repo = GlobalContainer.Resolve<IOrderRepository>();
                var order = repo.Get(id);

                order.Approve(SecurityContext.SecuritySession.User);
                repo.Update(order);

                new OrderApprovedMail()
                    .For(order)
                    .Send(order.Reserver);

                uow.TransactionalFlush();
            }
            return GetOrder(id);
        }

        public Stream GetPdf(int id)
        {
            // TODO: Prüfen ob Admin oder Besitzer der Bestellung
            using (UnitOfWork.Start())
            {
                var repo = GlobalContainer.Resolve<IOrderRepository>();
                var order = repo.Get(id);
                return order.GeneratePdf();
            }
        }

        #endregion

        private IList<OrderDto> GetClosedOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = GlobalContainer.Resolve<IOrderRepository>().FindClosed(SelectedOrganization);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }
    }
}