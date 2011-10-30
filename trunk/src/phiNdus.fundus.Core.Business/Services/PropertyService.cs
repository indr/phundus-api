using System;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class PropertyService : BaseService
    {
        private static IFieldDefinitionRepository PropertyDefinitions
        {
            get { return IoC.Resolve<IFieldDefinitionRepository>(); }
        }

        public virtual FieldDefinitionDto[] GetProperties()
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
    }
}