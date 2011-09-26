using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IPropertyService
    {
        PropertyDto[] GetProperties(string sessionKey);
    }
}