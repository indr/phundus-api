namespace Phundus.Core.Repositories
{
    using Entities;
    using Infrastructure;

    public interface IRoleRepository : IRepository<Role>
    {
        Role FindFirst(NHibernate.Criterion.Order sortOrder);
    }
}