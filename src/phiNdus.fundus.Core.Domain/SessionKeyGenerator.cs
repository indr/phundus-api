namespace phiNdus.fundus.Core.Domain
{
    public static class SessionKeyGenerator
    {
        public static string CreateKey()
        {
            return KeyGenerator.CreateKey(20);
        }
    }
}