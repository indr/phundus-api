namespace phiNdus.fundus.Web.Business.Services
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Business;
    using phiNdus.fundus.Web.Business.Assembler;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Repositories;
    using Phundus.Infrastructure;

    public class PropertyService : BaseService, IPropertyService
    {
        private static IFieldDefinitionRepository PropertyDefinitions
        {
            get { return ServiceLocator.Current.GetInstance<IFieldDefinitionRepository>(); }
        }

        #region IPropertyService Members

        public virtual IList<FieldDefinitionDto> GetProperties()
        {
            var propertyDefs = PropertyDefinitions.FindAll();
            return FieldDefinitionAssembler.CreateDtos(propertyDefs);
        }

        #endregion

        public virtual int CreateProperty(FieldDefinitionDto subject)
        {
            var propetyDef = FieldDefinitionAssembler.CreateDomainObject(subject);
            var id = PropertyDefinitions.Add(propetyDef).Id;
            return id;
        }

        public virtual FieldDefinitionDto GetProperty(int id)
        {
            var propertyDef = PropertyDefinitions.ById(id);
            if (propertyDef == null)
                return null;
            return FieldDefinitionAssembler.CreateDto(propertyDef);
        }

        public virtual void UpdateProperty(FieldDefinitionDto subject)
        {
            var propertyDef = FieldDefinitionAssembler.UpdateDomainObject(subject);
            PropertyDefinitions.Add(propertyDef);
        }

        public virtual void DeleteProperty(FieldDefinitionDto subject)
        {
            var propertyDef = FieldDefinitionAssembler.UpdateDomainObject(subject);
            if (propertyDef.IsSystem)
                throw new InvalidOperationException("System-Eigenschaften können nicht gelöscht werden.");

            PropertyDefinitions.Remove(propertyDef);
        }

        public void UpdateFields(IList<FieldDefinitionDto> subjects)
        {
            foreach (var each in subjects)
            {
                var fieldDefinition = PropertyDefinitions.ById(each.Id);
                Guard.Against<DtoOutOfDateException>(fieldDefinition.Version != each.Version, "Dto is out of date");
                fieldDefinition.IsDefault = each.IsDefault;
                fieldDefinition.Position = each.Position;
                fieldDefinition.IsColumn = each.IsColumn;
                PropertyDefinitions.Update(fieldDefinition);
            }
        }
    }

    public interface IPropertyService
    {
        IList<FieldDefinitionDto> GetProperties();
    }
}