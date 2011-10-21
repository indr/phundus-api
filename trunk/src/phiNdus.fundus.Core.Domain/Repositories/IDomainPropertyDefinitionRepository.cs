using System.Collections.Generic;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public interface IDomainPropertyDefinitionRepository : IRepository<FieldDefinition>
    {
        ICollection<FieldDefinition> FindAll();
    }
}