namespace Phundus.Core.Shop.Orders
{
    using System.Collections.Generic;
    using System.IO;
    using Model;
    using Queries;

    public interface IOrderService
    {
        OrderDto Reject(int id);
        OrderDto Confirm(int id);

        OrderDto GetOrder(int id);
        IEnumerable<OrderDto> GetOrders(OrderStatus status, int organizationId);
        IEnumerable<OrderDto> GetMyOrders();
        
        Stream GetPdf(int id);
    }
}