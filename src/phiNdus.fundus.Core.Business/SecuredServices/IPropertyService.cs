using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IPropertyService
    {
        FieldDefinitionDto[] GetProperties(string sessionKey);
        void UpdateProperty(string sessionKey, FieldDefinitionDto subject);
        FieldDefinitionDto GetProperty(string sessionKey, int id);
        int CreateProperty(string sessionKey, FieldDefinitionDto subject);
        void DeleteProperty(string sessionKey, FieldDefinitionDto subject);
    }
}