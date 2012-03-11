using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
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
                return new OrderAssembler().CreateDto(order);
            }
        }

        public virtual IList<OrderDto> GetPending()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindPending();
                return new OrderAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetApproved()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindApproved();
                return new OrderAssembler().CreateDtos(orders);
            }
            
        }

        public virtual IList<OrderDto> GetRejected()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindRejected();
                return new OrderAssembler().CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindAll();
                return new OrderAssembler().CreateDtos(orders);
            }
        }

        public OrderDto GetCart()
        {
            using (UnitOfWork.Start())
            {
                var order = IoC.Resolve<IOrderRepository>().FindCart(SecurityContext.SecuritySession.User.Id);
                if (order == null)
                    order = new Order();
                return new OrderAssembler().CreateDto(order);
            }
        }

        public OrderDto AddToCart(OrderItemDto orderItemDto)
        {
            Guard.Against<ArgumentNullException>(orderItemDto == null, "orderItemDto");
            Guard.Against<ArgumentException>(orderItemDto.OrderId <= 0, "OrderId cannot be less or equal 0");
            Guard.Against<ArgumentException>(orderItemDto.Amount <= 0, "Amount cannot be less or equal 0");
            Guard.Against<ArgumentException>(orderItemDto.ArticleId <= 0, "ArticleId cannot be less or equal 0");


            using (var uow = UnitOfWork.Start())
            {
                var repo = IoC.Resolve<IOrderRepository>();
                var order = repo.Get(orderItemDto.OrderId);
                Guard.Against<ArgumentException>(order == null, "Order not found");

                order.AddItem(orderItemDto.ArticleId, orderItemDto.Amount, orderItemDto.From, orderItemDto.To);

                uow.TransactionalFlush();

                // TODO: Hmm... Nach Commit, neue Transaktion oder wie?
                return new OrderAssembler().CreateDto(repo.Get(order.Id));
            }
        }
    }
}