namespace Phundus.Core.OrganisationCtx.Repositories
{
    using DomainModel;

    public interface IOrganisationRepository
    {
        Organisation ById(int id);
    }
}