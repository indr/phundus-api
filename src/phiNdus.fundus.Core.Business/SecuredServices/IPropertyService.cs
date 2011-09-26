using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IPropertyService
    {
        PropertyDto[] GetProperties(string sessionKey);
        void UpdateProperty(string sessionKey, PropertyDto subject);
        PropertyDto GetProperty(string sessionKey, int id);
        int CreateProperty(string sessionKey, PropertyDto subject);
        void DeleteProperty(string sessionKey, PropertyDto subject);
    }
}