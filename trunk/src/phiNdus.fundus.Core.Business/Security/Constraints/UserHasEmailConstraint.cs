using System;

namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class UserHasEmailConstraint : AbstractConstraint
    {
        private readonly string _email;

        public UserHasEmailConstraint(string email)
        {
            _email = email;
        }

        public override bool Eval(SecurityContext context)
        {
            return context.SecuritySession.User.Membership.Email.Equals(_email, StringComparison.OrdinalIgnoreCase);
        }

        public override string Message
        {
            get { return "UserHasEmailConstraint"; }
        }
    }
}