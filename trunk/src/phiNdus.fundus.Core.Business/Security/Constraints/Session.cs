namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public static class Session
    {
        public static AbstractConstraint FromKey(string key)
        {
            return new SessionFromKeyConstraint(key);
        }
    }
}