namespace phiNdus.fundus.Web.Business.Services
{
    using System;
    using System.Collections.Generic;
    using Assembler;
    using Dto;
    using fundus.Business;
    using phiNdus.fundus.Domain.Repositories;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class PropertyService : BaseService, IPropertyService
    {
        private static IFieldDefinitionRepository PropertyDefinitions
        {
            get { return GlobalContainer.Resolve<IFieldDefinitionRepository>(); }
        }

        #region IPropertyService Members

        public virtual IList<FieldDefinitionDto> GetProperties()
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDefs = PropertyDefinitions.FindAll();
                return FieldDefinitionAssembler.CreateDtos(propertyDefs);
            }
        }

        #endregion

        public virtual int CreateProperty(FieldDefinitionDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propetyDef = FieldDefinitionAssembler.CreateDomainObject(subject);
                var id = PropertyDefinitions.Save(propetyDef).Id;
                uow.TransactionalFlush();
                return id;
            }
        }

        public virtual FieldDefinitionDto GetProperty(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDef = PropertyDefinitions.Get(id);
                if (propertyDef == null)
                    return null;
                return FieldDefinitionAssembler.CreateDto(propertyDef);
            }
        }

        public virtual void UpdateProperty(FieldDefinitionDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDef = FieldDefinitionAssembler.UpdateDomainObject(subject);
                PropertyDefinitions.Save(propertyDef);
                uow.TransactionalFlush();
            }
        }

        public virtual void DeleteProperty(FieldDefinitionDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDef = FieldDefinitionAssembler.UpdateDomainObject(subject);
                if (propertyDef.IsSystem)
                    throw new InvalidOperationException("System-Eigenschaften können nicht gelöscht werden.");

                PropertyDefinitions.Delete(propertyDef);
                uow.TransactionalFlush();
            }
        }

        public void UpdateFields(IList<FieldDefinitionDto> subjects)
        {
            using (var uow = UnitOfWork.Start())
            {
                foreach (var each in subjects)
                {
                    var fieldDefinition = PropertyDefinitions.Get(each.Id);
                    Guard.Against<DtoOutOfDateException>(fieldDefinition.Version != each.Version, "Dto is out of date");
                    fieldDefinition.IsDefault = each.IsDefault;
                    fieldDefinition.Position = each.Position;
                    fieldDefinition.IsColumn = each.IsColumn;
                    PropertyDefinitions.Update(fieldDefinition);
                }
                uow.TransactionalFlush();
            }
        }
    }

    public interface IPropertyService
    {
        IList<FieldDefinitionDto> GetProperties();
    }
}