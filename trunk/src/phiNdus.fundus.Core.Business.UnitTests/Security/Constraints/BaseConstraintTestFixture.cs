using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.TestHelpers.Builders;

namespace phiNdus.fundus.Core.Business.UnitTests.Security.Constraints
{
    public class BaseConstraintTestFixture
    {
        protected SecurityContext SecurityContext(User user)
        {
            return SecurityContext(user, null);
        }
        protected SecurityContext SecurityContext(User user, string key)
        {
            var result = new SecurityContext();
            result.SecuritySession = new SecuritySessionBuilder(user, key).Build();
            return result;
        }
    }
}