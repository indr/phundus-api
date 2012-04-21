using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;

namespace phiNdus.fundus.Business.UnitTests.Security.Constraints
{
    public class AlwaysFalseConstraint : AbstractConstraint
    {
        public override bool Eval(SecurityContext context)
        {
            return false;
        }

        public override string Message
        {
            get { return "AlwaysFalseConstraint"; }
        }
    }
}