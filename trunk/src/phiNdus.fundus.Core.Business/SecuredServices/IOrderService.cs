using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IOrderService
    {
        OrderDto GetOrder(string sessionKey, int id);
        IList<OrderDto> GetPendingOrders(string sessionKey);
        IList<OrderDto> GetApprovedOrders(string sessionKey);
        IList<OrderDto> GetRejectedOrders(string sessionKey);
    }
}