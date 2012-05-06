using System.Collections.Generic;
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
        void CheckOut(string sessionKey);
        IList<OrderDto> GetOrders(string sessionKey, OrderStatus status);
    }
}