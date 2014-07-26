namespace Phundus.Core.IdentityAndAccessCtx.Repositories
{
    using System.Collections.Generic;
    using DomainModel;
    using Infrastructure;

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