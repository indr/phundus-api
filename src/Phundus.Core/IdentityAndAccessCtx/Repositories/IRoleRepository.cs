namespace Phundus.Core.IdentityAndAccessCtx.Repositories
{
    using Core.Repositories;
    using DomainModel;

    public interface IRoleRepository : IRepository<Role>
    {
        Role FindFirst(NHibernate.Criterion.Order sortOrder);
    }
}