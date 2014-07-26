﻿namespace Phundus.Persistence.Repositories
{
    using Core.IdentityAndAccess.Users.Model;
    using Core.IdentityAndAccess.Users.Repositories;
    using phiNdus.fundus.Domain.Repositories;
    using Phundus.Persistence;

    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public Role FindFirst(NHibernate.Criterion.Order sortOrder)
        {
            var crit = Session.CreateCriteria<Role>();
            if (sortOrder != null)
                crit.AddOrder(sortOrder);
            crit.SetFirstResult(0);
            crit.SetMaxResults(1);
            return crit.UniqueResult<Role>();
        }
    }
}