namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class UserIsContraint : AbstractConstraint
    {
        private readonly int _id;

        public UserIsContraint(int id)
        {
            _id = id;
        }

        public override string Message
        {
            get { return "UserIsConstraint"; }
        }

        public override bool Eval(SecurityContext context)
        {
            return context.SecuritySession.User.Id.Equals(_id);
        }
    }
}