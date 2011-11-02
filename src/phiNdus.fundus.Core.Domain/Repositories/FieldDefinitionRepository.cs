using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class FieldDefinitionRepository : NHRepository<FieldDefinition>, IFieldDefinitionRepository
    {
        private IQueryable<FieldDefinition> FieldDefinitions
        {
            get { return Session.Query<FieldDefinition>(); }
        }

        #region IFieldDefinitionRepository Members

        public ICollection<FieldDefinition> FindAll()
        {
            return (from fd in FieldDefinitions orderby fd.Position ascending select fd).ToList();
        }

        #endregion
    }
}