using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class User
    {
        public static AbstractConstraint InRole(Role role)
        {
            return new UserInRoleConstraint(role);
        }

        public static AbstractConstraint HasEmail(string email)
        {
            return new UserHasEmailConstraint(email);
        }
    }
}