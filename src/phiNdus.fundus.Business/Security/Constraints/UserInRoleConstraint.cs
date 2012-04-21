using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.Security.Constraints
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