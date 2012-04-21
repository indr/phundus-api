using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;

namespace phiNdus.fundus.Business.UnitTests.Security.Constraints
{
    public class AlwaysTrueConstraint : AbstractConstraint
    {
        public override bool Eval(SecurityContext context)
        {
            return true;
        }

        public override string Message
        {
            get { return "AlwaysTrueConstraint"; }
        }
    }
}