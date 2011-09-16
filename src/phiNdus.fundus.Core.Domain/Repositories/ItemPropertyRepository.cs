using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class ItemPropertyRepository : NHRepository<ItemProperty>, IItemPropertyRepository
    {
        private IQueryable<ItemProperty> ItemProperties
        {
            get { return Session.Query<ItemProperty>(); }
        }

        #region IItemPropertyRepository Members

        public ICollection<ItemProperty> FindAll()
        {
            var query = from u in ItemProperties select u;
            return query.ToList();
        }

        #endregion
    }
}