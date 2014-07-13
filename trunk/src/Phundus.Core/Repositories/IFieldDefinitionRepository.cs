namespace Phundus.Core.Repositories
{
    using System.Collections.Generic;
    using Phundus.Core.Entities;

    public interface IFieldDefinitionRepository : IRepository<FieldDefinition>
    {
        ICollection<FieldDefinition> FindAll();
    }
}