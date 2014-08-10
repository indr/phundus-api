namespace Phundus.Core.Shop.Queries.Models
{
    using System.Collections.Generic;
    using Orders.Model;
    using Orders.Repositories;

    public class OrderReadModelReader : ReadModelReaderBase, IOrderQueries
    {
        public IOrderRepository OrderRepository { get; set; }

        public OrderDto FindById(int orderId, int userId)
        {
            var order = OrderRepository.ById(orderId);
            if (order == null)
                return null;
            return new OrderDtoAssembler().CreateDto(order);
        }

        public IEnumerable<OrderDto> FindByOrganizationId(int organizationId, int userId, OrderStatus status)
        {
            var orders = OrderRepository.FindByOrganizationId(organizationId, status);
            return new OrderDtoAssembler().CreateDtos(orders);
        }

        public IEnumerable<OrderDto> FindByUserId(int userId)
        {
            var orders = OrderRepository.FindByUserId(userId);
            return new OrderDtoAssembler().CreateDtos(orders);
        }
    }
}