namespace Phundus.Persistence.Legacy.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using NHibernate.Linq;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;

    public class FieldDefinitionRepository : RepositoryBase<FieldDefinition>, IFieldDefinitionRepository
    {
        private IQueryable<FieldDefinition> FieldDefinitions
        {
            get { return Session.Query<FieldDefinition>(); }
        }

        #region IFieldDefinitionRepository Members

        public ICollection<FieldDefinition> FindAll()
        {
            return (from fd in FieldDefinitions orderby fd.Position, fd.Id select fd).ToList();
        }

        #endregion
    }
}