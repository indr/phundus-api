namespace Phundus.Core.OrganisationCtx.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;

    public interface IOrganizationRepository : IRepository<Organization>
    {
        ICollection<Organization> FindAll();
        Organization FindById(int id);
        Organization FindByName(string name);
    }
}