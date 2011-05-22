using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;

namespace phiNdus.fundus.Core.Business.UnitTests.Security.Constraints
{
    public class AlwaysTrueConstraint : AbstractConstraint
    {
        public override bool Eval(SecurityContext context)
        {
            return true;
        }
    }
}