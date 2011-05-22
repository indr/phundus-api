using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.UnitTests.Security.Constraints
{
    internal class BaseConstraintTestFixture
    {
        protected SecurityContext SecurityContext(User user)
        {
            return SecurityContext(user, null);
        }
        protected SecurityContext SecurityContext(User user, string key)
        {
            var result = new SecurityContext();
            result.SecuritySession = SessionHelper.CreateSession(user, key);
            return result;
        }
    }
}