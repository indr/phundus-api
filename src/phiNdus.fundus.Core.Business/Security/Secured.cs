using phiNdus.fundus.Core.Business.Security.Constraints;

namespace phiNdus.fundus.Core.Business.Security
{
    public class Secured
    {
        public static SecuredHelper With(AbstractConstraint constraint)
        {
            return new SecuredHelper(constraint);
        }
    }
}