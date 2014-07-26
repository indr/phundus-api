namespace Phundus.Core.OrganizationAndMembershipCtx.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IOrganizationRepository : IRepository<Organization>
    {
        ICollection<Organization> FindAll();
        Organization FindById(int id);
        Organization FindByName(string name);
    }
}