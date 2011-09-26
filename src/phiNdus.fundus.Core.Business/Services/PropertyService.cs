using System;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class PropertyService : BaseService
    {
        private static IDomainPropertyDefinitionRepository PropertyDefinitions
        {
            get { return IoC.Resolve<IDomainPropertyDefinitionRepository>(); }
        }

        public virtual PropertyDto[] GetProperties()
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDefs = PropertyDefinitions.FindAll();
                return PropertyDefinitionAssembler.CreateDtos(propertyDefs);
            }
        }

        public virtual int CreateProperty(PropertyDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propetyDef = PropertyDefinitionAssembler.CreateDomainObject(subject);
                var id = PropertyDefinitions.Save(propetyDef).Id;
                uow.TransactionalFlush();
                return id;
            }
        }

        public virtual PropertyDto GetProperty(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDef = PropertyDefinitions.Get(id);
                if (propertyDef == null)
                    return null;
                return PropertyDefinitionAssembler.CreateDto(propertyDef);
            }
        }

        public virtual void UpdateProperty(PropertyDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyDef = PropertyDefinitionAssembler.UpdateDomainObject(subject);
                PropertyDefinitions.Save(propertyDef);
                uow.TransactionalFlush();
            }
        }
    }
}