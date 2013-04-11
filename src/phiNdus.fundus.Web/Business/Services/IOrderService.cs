namespace phiNdus.fundus.Business.Services
{
    using System.Collections.Generic;
    using System.IO;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Business.Dto;

    public interface IOrderService
    {
        OrderDto Reject(int id);
        OrderDto Confirm(int id);

        OrderDto GetOrder(int id);
        IList<OrderDto> GetOrders(OrderStatus status);
        IList<OrderDto> GetMyOrders();
        IList<OrderDto> GetPendingOrders();
        IList<OrderDto> GetApprovedOrders();
        IList<OrderDto> GetRejectedOrders();
        Stream GetPdf(int id);
    }
}