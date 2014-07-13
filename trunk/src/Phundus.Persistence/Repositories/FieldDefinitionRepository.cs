using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace phiNdus.fundus.Domain.Repositories
{
    using Phundus.Core.Entities;
    using Phundus.Core.Repositories;
    using Phundus.Persistence;

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