using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Repositories
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public class FieldDefinitionRepository : NHRepository<FieldDefinition>, IFieldDefinitionRepository
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