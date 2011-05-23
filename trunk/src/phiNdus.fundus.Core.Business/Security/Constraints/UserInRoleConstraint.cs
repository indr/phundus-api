using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class UserInRoleConstraint : AbstractConstraint
    {
        private readonly Role _role;

        public UserInRoleConstraint(Role role)
        {
            _role = role;
        }

        public override bool Eval(SecurityContext context)
        {
            return context.SecuritySession.User.Role.Equals(_role);
        }

        public override string Message
        {
            get { return "UserInRoleConstraint"; }
        }
    }
}