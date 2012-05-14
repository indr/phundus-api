using System.Collections.Generic;
using System.IO;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.SecuredServices
{
    public interface IOrderService
    {
        OrderDto GetOrder(string sessionKey, int id);
        IList<OrderDto> GetPendingOrders(string sessionKey);
        IList<OrderDto> GetApprovedOrders(string sessionKey);
        IList<OrderDto> GetRejectedOrders(string sessionKey);
        IList<OrderDto> GetMyOrders(string sessionKey);

        IList<OrderDto> GetOrders(string sessionKey, OrderStatus status);

        void CheckOut(string sessionKey);
        OrderDto Reject(string sessionId, int id);
        OrderDto Confirm(string sessionId, int id);
        Stream GetPdf(string sessionId, int id);
    }
}