namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using Entities;
    using Phundus.Infrastructure;

    public interface IUserRepository : IRepository<User>
    {
        ICollection<User> FindAll();
        User FindByEmail(string email);
        User FindBySessionKey(string sessionKey);
        User FindByValidationKey(string validationKey);

        ICollection<User> FindByOrganization(int organizationId);
        User FindById(int id);
    }
}