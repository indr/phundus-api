using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
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
                return OrderAssembler.CreateDto(order);
            }
        }

        public virtual IList<OrderDto> GetPending()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindPending();
                return OrderAssembler.CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetApproved()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindApproved();
                return OrderAssembler.CreateDtos(orders);
            }
            
        }

        public virtual IList<OrderDto> GetRejected()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindRejected();
                return OrderAssembler.CreateDtos(orders);
            }
        }

        public virtual IList<OrderDto> GetOrders()
        {
            using (UnitOfWork.Start())
            {
                var orders = IoC.Resolve<IOrderRepository>().FindAll();
                return OrderAssembler.CreateDtos(orders);
            }
        }
    }
}