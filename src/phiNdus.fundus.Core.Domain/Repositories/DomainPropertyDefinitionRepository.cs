using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class DomainPropertyDefinitionRepository : NHRepository<DomainPropertyDefinition>, IDomainPropertyDefinitionRepository
    {
        private IQueryable<DomainPropertyDefinition> ItemProperties
        {
            get { return Session.Query<DomainPropertyDefinition>(); }
        }

        #region IDomainPropertyDefinitionRepository Members

        public ICollection<DomainPropertyDefinition> FindAll()
        {
            var query = from u in ItemProperties select u;
            return query.ToList();
        }

        #endregion
    }
}