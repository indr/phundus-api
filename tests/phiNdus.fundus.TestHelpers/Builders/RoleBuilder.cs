using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.TestHelpers.Builders
{
    public class RoleBuilder
    {
        public Role User()
        {
            return UnitOfWork.CurrentSession.Get<Role>(Role.User.Id);
        }
    }
}