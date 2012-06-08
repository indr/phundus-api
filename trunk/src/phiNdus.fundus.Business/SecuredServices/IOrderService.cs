using System.Collections.Generic;
using System.IO;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.SecuredServices
{
    public interface IOrderService
    {
        //void CheckOut(string sessionId);
        OrderDto Reject(string sessionId, int id);
        OrderDto Confirm(string sessionId, int id);
        
        OrderDto GetOrder(string sessionKey, int id);
        IList<OrderDto> GetOrders(string sessionKey, OrderStatus status);
        IList<OrderDto> GetMyOrders(string sessionKey);
        IList<OrderDto> GetPendingOrders(string sessionKey);
        IList<OrderDto> GetApprovedOrders(string sessionKey);
        IList<OrderDto> GetRejectedOrders(string sessionKey);
        Stream GetPdf(string sessionId, int id);
    }
}