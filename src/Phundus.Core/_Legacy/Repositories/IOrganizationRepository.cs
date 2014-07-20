namespace Phundus.Core.Repositories
{
    using System.Collections.Generic;
    using Entities;

    public interface IOrganizationRepository : IRepository<Organization>
    {
        ICollection<Organization> FindAll();
        Organization FindById(int id);
        Organization FindByName(string name);
    }
}