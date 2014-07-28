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
        IList<OrderDto> GetOrders(OrderStatus status, int organizationId);
        IList<OrderDto> GetMyOrders();
        IList<OrderDto> GetPendingOrders(int organizationId);
        IList<OrderDto> GetApprovedOrders(int organizationId);
        IList<OrderDto> GetRejectedOrders(int organizationId);
        Stream GetPdf(int id);
    }
}