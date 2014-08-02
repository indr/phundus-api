namespace Phundus.Core.Inventory._Legacy.Services
{
    using System.Collections.Generic;
    using Dtos;

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