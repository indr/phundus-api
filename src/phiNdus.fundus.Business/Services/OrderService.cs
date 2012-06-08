using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Mails;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.Domain.Settings;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services
{
    public class OrderService : BaseService
    {
        public virtual OrderDto GetOrder(int id)
        {
            using (UnitOfWork.Start())
            {
                var order = IoC.Resolve<IOrderRepository>().Get(id);
                if (order == null)
                    return null;
                return new OrderDtoAssembler().CreateDto(order);
            }
        }

        public virtual IList<OrderDto> GetPending()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindPending();
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetApproved()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindApproved();
                return new OrderDtoAssembler().CreateDtos(orders);
            }
            
        }

        public virtual IList<OrderDto> GetRejected()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindRejected();
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        private IList<OrderDto> GetClosed()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindClosed();
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetMyOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindMy(SecurityContext.SecuritySession.User.Id);
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public IList<OrderDto> GetOrders(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Pending:
                    return GetPending();
                    break;
                case OrderStatus.Approved:
                    return GetApproved();
                    break;
                case OrderStatus.Rejected:
                    return GetRejected();
                    break;
                case OrderStatus.Closed:
                    return GetClosed();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }


        public OrderDto Reject(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var repo = IoC.Resolve<IOrderRepository>();
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
                var repo = IoC.Resolve<IOrderRepository>();
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
                var repo = IoC.Resolve<IOrderRepository>();
                var order = repo.Get(id);
                return order.GeneratePdf();
            }
        }
    }
}