using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IFieldsService
    {
        FieldDefinitionDto[] GetProperties(string sessionKey);
        void UpdateField(string sessionKey, FieldDefinitionDto subject);
        FieldDefinitionDto GetField(string sessionKey, int id);
        int CreateField(string sessionKey, FieldDefinitionDto subject);
        void DeleteField(string sessionKey, FieldDefinitionDto subject);
    }
}