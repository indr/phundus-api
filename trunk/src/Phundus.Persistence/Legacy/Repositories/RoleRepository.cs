namespace Phundus.Persistence.Legacy.Repositories
{
    using Infrastructure;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;
    using Order = NHibernate.Criterion.Order;

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