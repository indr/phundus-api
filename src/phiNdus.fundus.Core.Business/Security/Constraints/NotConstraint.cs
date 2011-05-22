namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class NotConstraint : AbstractConstraint
    {
        private readonly AbstractConstraint _c1;

        public NotConstraint(AbstractConstraint c1)
        {
            _c1 = c1;
        }

        public override bool Eval(SecurityContext context)
        {
            return !_c1.Eval(context);
        }
    }
}