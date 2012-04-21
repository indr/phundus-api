using System;
using System.Collections.Generic;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services
{
    public class PropertyService : BaseService
    {
        private static IFieldDefinitionRepository PropertyDefinitions
        {
            get { return IoC.Resolve<IFieldDefinitionRepository>(); }
        }

        public virtual IList<FieldDefinitionDto> GetProperties()
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDefs = PropertyDefinitions.FindAll();
                return FieldDefinitionAssembler.CreateDtos(propertyDefs);
            }
        }

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
}