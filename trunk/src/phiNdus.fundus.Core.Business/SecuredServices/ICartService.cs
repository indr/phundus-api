using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface ICartService
    {
        OrderDto GetCart(string sessionKey);
    }
}