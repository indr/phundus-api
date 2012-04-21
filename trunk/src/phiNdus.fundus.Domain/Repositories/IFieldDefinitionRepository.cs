using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface IFieldDefinitionRepository : IRepository<FieldDefinition>
    {
        ICollection<FieldDefinition> FindAll();
    }
}