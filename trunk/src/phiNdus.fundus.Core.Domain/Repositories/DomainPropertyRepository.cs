using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class DomainPropertyRepository : NHRepository<DomainProperty>, IDomainPropertyRepository
    {
        private IQueryable<DomainProperty> ItemProperties
        {
            get { return Session.Query<DomainProperty>(); }
        }

        #region IDomainPropertyRepository Members

        public ICollection<DomainProperty> FindAll()
        {
            var query = from u in ItemProperties select u;
            return query.ToList();
        }

        #endregion
    }
}