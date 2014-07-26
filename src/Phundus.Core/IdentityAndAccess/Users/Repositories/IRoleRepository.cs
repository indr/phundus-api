namespace Phundus.Core.IdentityAndAccess.Users.Repositories
{
    using Infrastructure;
    using Model;

    public interface IRoleRepository : IRepository<Role>
    {
        Role FindFirst(NHibernate.Criterion.Order sortOrder);
    }
}