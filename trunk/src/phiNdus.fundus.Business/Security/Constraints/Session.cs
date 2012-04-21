namespace phiNdus.fundus.Business.Security.Constraints
{
    public static class Session
    {
        public static AbstractConstraint FromKey(string key)
        {
            return new SessionFromKeyConstraint(key);
        }
    }
}