namespace phiNdus.fundus.Business.Services
{
    using System.Collections.Generic;
    using Dto;

    public interface IFieldsService
    {
        IList<FieldDefinitionDto> GetProperties();
        void UpdateField(FieldDefinitionDto subject);
        FieldDefinitionDto GetField(int id);
        int CreateField(FieldDefinitionDto subject);
        void DeleteField(FieldDefinitionDto subject);
        void UpdateFields(IList<FieldDefinitionDto> subjects);
    }
}