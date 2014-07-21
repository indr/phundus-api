namespace Phundus.Core.IdentityAndAccessCtx.Queries
{
    using DomainModel;

    public interface IUserQueries
    {
        User ById(int id);
    }
}