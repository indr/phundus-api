namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public class OrConstraint : AbstractConstraint
    {
        private readonly AbstractConstraint _c1;
        private readonly AbstractConstraint _c2;
        private string _message;

        public OrConstraint(AbstractConstraint c1, AbstractConstraint c2)
        {
            _c1 = c1;
            _c2 = c2;
            _message = "not yet evaluated";
        }

        public override bool Eval(SecurityContext context)
        {
            var c1 = _c1.Eval(context);
            if (c1)
            {
                _message = "";
                return true;
            }
            
            var c2 = _c2.Eval(context);
            if (c2)
            {
                _message = "";
                return true;
            }

            _message = _c1.Message + " or " + _c2.Message;
            return false;
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}