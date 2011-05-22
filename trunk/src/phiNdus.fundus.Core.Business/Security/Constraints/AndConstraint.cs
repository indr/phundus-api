namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class AndConstraint : AbstractConstraint
    {
        private readonly AbstractConstraint _c1;
        private readonly AbstractConstraint _c2;

        public AndConstraint(AbstractConstraint c1, AbstractConstraint c2)
        {
            _c1 = c1;
            _c2 = c2;
        }

        public override bool Eval(SecurityContext context)
        {
            return _c1.Eval(context) && _c2.Eval(context);
        }
    }
}