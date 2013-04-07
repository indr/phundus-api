namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using Entities;
    using piNuts.phundus.Infrastructure.Obsolete;

    public interface IMemberRepository : IRepository<User>
    {
        ICollection<User> FindByOrganization(int organizationId);
    }
}