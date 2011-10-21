using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class DomainPropertyDefinitionRepository : NHRepository<FieldDefinition>, IDomainPropertyDefinitionRepository
    {
        private IQueryable<FieldDefinition> ItemProperties
        {
            get { return Session.Query<FieldDefinition>(); }
        }

        #region IDomainPropertyDefinitionRepository Members

        public ICollection<FieldDefinition> FindAll()
        {
            var query = from u in ItemProperties select u;
            return query.ToList();
        }

        #endregion
    }
}