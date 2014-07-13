namespace phiNdus.fundus.Domain.Repositories
{
    using phiNdus.fundus.Domain.Entities;
    using Phundus.Infrastructure;
    using Order = NHibernate.Criterion.Order;

    public interface IRoleRepository : IRepository<Role>
    {
        Role FindFirst(Order sortOrder);
    }
}