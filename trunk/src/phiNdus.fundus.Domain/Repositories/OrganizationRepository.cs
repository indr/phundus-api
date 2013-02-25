﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public class OrganizationRepository : NHRepository<Organization>, IOrganizationRepository
    {
        private IQueryable<Organization> Organizations
        {
            get { return Session.Query<Organization>(); }
        }

        #region IOrganizationRepository Members

        public ICollection<Organization> FindAll()
        {
            var query = from o in Organizations select o;
            return query.ToList();
        }

        public Organization FindById(int id)
        {
            var query = from o in Organizations where o.Id == id select o;
            return query.SingleOrDefault();
        }

        #endregion
    }

    public interface IOrganizationRepository : IRepository<Organization>
    {
        ICollection<Organization> FindAll();
        Organization FindById(int id);
    }
}