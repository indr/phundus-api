namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using phiNdus.fundus.Domain.Entities;
    using piNuts.phundus.Infrastructure;

    public interface IMemberRepository : IRepository<User>
    {
        ICollection<User> FindByOrganization(int organizationId);
        User FindById(int id);
    }
}