namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class AndConstraint : AbstractConstraint
    {
        private readonly AbstractConstraint _c1;
        private readonly AbstractConstraint _c2;
        private string _message;

        public AndConstraint(AbstractConstraint c1, AbstractConstraint c2)
        {
            _c1 = c1;
            _c2 = c2;
            _message = "not yet evaluated";
        }

        public override bool Eval(SecurityContext context)
        {
            if (!_c1.Eval(context))
            {
                _message = _c1.Message;
                return false;
            }
            if (!_c2.Eval(context))
            {
                _message = _c2.Message;
                return false;
            }
            return true;
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}