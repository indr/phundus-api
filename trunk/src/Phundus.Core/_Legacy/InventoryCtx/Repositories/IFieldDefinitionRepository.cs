namespace Phundus.Core.InventoryCtx.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IFieldDefinitionRepository : IRepository<FieldDefinition>
    {
        ICollection<FieldDefinition> FindAll();
    }
}