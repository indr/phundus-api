using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.Builders
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public class RoleBuilder
    {
        public Role User()
        {
            return UnitOfWork.CurrentSession.Get<Role>(Role.User.Id);
        }

        public Role Admin()
        {
            return UnitOfWork.CurrentSession.Get<Role>(Role.Administrator.Id);
        }
    }
}