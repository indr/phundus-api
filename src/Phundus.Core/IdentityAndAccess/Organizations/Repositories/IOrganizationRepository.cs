namespace Phundus.Core.IdentityAndAccess.Organizations.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrganizationRepository : IRepository<Organization>
    {
        Organization GetById(int id);

        Organization FindByName(string name);
        ICollection<Organization> FindAll();
    }
}