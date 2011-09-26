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
    }
}