using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;

namespace phiNdus.fundus.Core.Business.UnitTests.Security.Constraints
{
    public class AlwaysFalseConstraint : AbstractConstraint
    {
        public override bool Eval(SecurityContext context)
        {
            return false;
        }
    }
}