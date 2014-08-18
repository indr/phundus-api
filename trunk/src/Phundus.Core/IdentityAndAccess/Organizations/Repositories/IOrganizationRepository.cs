namespace Phundus.Core.IdentityAndAccess.Organizations.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrganizationRepository : IRepository<Organization>
    {
        ICollection<Organization> FindAll();
        Organization FindById(int id);
        Organization FindByName(string name);
        Organization ById(object id);
        Organization GetById(object id);
    }
}