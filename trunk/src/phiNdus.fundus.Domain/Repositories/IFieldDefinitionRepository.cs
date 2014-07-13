namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using phiNdus.fundus.Domain.Entities;
    using Phundus.Infrastructure;

    public interface IFieldDefinitionRepository : IRepository<FieldDefinition>
    {
        ICollection<FieldDefinition> FindAll();
    }
}