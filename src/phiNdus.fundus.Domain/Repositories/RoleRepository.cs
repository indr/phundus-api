using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Repositories
{
    using NHibernate.Criterion;
    using Phundus.Infrastructure;

    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public Role FindFirst(Order sortOrder)
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