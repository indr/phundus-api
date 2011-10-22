using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class FieldDefinitionRepository : NHRepository<FieldDefinition>, IFieldDefinitionRepository
    {
        private IQueryable<FieldDefinition> ItemProperties
        {
            get { return Session.Query<FieldDefinition>(); }
        }

        #region IFieldDefinitionRepository Members

        public ICollection<FieldDefinition> FindAll()
        {
            var query = from u in ItemProperties select u;
            return query.ToList();
        }

        #endregion
    }
}