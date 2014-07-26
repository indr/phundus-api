namespace Phundus.Core.IdentityAndAccessCtx.Repositories
{
    using DomainModel;
    using Phundus.Infrastructure;

    public interface IRoleRepository : IRepository<Role>
    {
        Role FindFirst(NHibernate.Criterion.Order sortOrder);
    }
}