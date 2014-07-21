namespace Phundus.Core.InventoryCtx
{
    using System.Collections.Generic;
    using Infrastructure;

    public interface IFieldDefinitionRepository : IRepository<FieldDefinition>
    {
        ICollection<FieldDefinition> FindAll();
    }
}