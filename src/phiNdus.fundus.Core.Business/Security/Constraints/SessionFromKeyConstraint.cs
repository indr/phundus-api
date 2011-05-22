namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    internal class SessionFromKeyConstraint : AbstractConstraint
    {
        private readonly string _key;

        public SessionFromKeyConstraint(string key)
        {
            _key = key;
        }

        public override bool Eval(SecurityContext context)
        {
            var securitySession = SecuritySession.FromKey(_key);
            context.SecuritySession = securitySession;
            return true;
        }
    }
}