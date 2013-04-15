using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Repositories
{
    using NHibernate.Criterion;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public interface IRoleRepository : IRepository<Role>
    {
        Role FindFirst(Order sortOrder);
    }
}