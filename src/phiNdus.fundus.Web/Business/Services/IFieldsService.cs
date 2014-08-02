namespace phiNdus.fundus.Web.Business.Services
{
    using System.Collections.Generic;
    using Phundus.Core.Inventory._Legacy.Dtos;

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