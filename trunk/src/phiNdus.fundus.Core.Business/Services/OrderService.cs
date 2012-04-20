using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual IList<OrderDto> GetOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindAll();
                return new OrderDtoAssembler().CreateDtos(orders);
            }
        }

        public OrderDto GetCart()
        {
            using (UnitOfWork.Start())
            {
                var order = GetOrCreateCart();
                return new OrderDtoAssembler().CreateDto(order);
            }
        }

        private Order FindCart()
        {
            return IoC.Resolve<IOrderRepository>().FindCart(SecurityContext.SecuritySession.User.Id);
        }

        private Order GetOrCreateCart()
        {
            var order = FindCart();
            if (order == null)
                order = new Order();
            return order;
        }

        public OrderDto AddToCart(OrderItemDto orderItemDto)
        {
            Guard.Against<ArgumentNullException>(orderItemDto == null, "orderItemDto");
            Guard.Against<ArgumentException>(orderItemDto.Amount <= 0, "Amount cannot be less or equal 0");
            Guard.Against<ArgumentException>(orderItemDto.ArticleId <= 0, "ArticleId cannot be less or equal 0");


            using (var uow = UnitOfWork.Start())
            {
                Order order;
                var orderRepo = IoC.Resolve<IOrderRepository>();
                if (orderItemDto.OrderId > 0)
                {
                    order = orderRepo.Get(orderItemDto.OrderId);
                    Guard.Against<ArgumentException>(order == null, "Order not found");
                }
                else
                {
                    order = GetOrCreateCart();
                    order.Reserver = SecurityContext.SecuritySession.User;
                }

                order.AddItem(orderItemDto.ArticleId, orderItemDto.Amount, orderItemDto.From, orderItemDto.To);
                orderRepo.Save(order);

                uow.TransactionalFlush();
            }
            return null;
        }

        public void RemoveFromCart(int orderItemId, int version)
        {
            using (var uow = UnitOfWork.Start())
            {
                var cart = FindCart();
                if (cart == null)
                    return;

                var orderItem = cart.Items.FirstOrDefault(x => x.Id == orderItemId && x.Version == version);
                if (orderItem == null)
                    return;
                
                cart.RemoveItem(orderItem);
                IoC.Resolve<IOrderRepository>().Save(cart);
                uow.TransactionalFlush();
            }
        }
    }
}