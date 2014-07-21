﻿namespace Phundus.Persistence.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Core.OrganizationAndMembershipCtx.Model;
    using Core.OrganizationAndMembershipCtx.Repositories;
    using NHibernate.Linq;
    using Phundus.Persistence;

    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        private IQueryable<Organization> Organizations
        {
            get { return Session.Query<Organization>(); }
        }

        #region IOrganizationRepository Members

        [Transaction]
        public ICollection<Organization> FindAll()
        {
            var query = from o in Organizations where o.Name != "Reserved" select o;
            return query.ToList();
        }

        public Organization FindById(int id)
        {
            var query = from o in Organizations where o.Id == id select o;
            return query.SingleOrDefault();
        }

        public Organization FindByName(string name)
        {
            var query = from o in Organizations where o.Name == name select o;
            return query.SingleOrDefault();
        }

        #endregion
    }
}