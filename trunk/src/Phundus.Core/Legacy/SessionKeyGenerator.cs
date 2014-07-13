namespace phiNdus.fundus.Domain
{
    public static class SessionKeyGenerator
    {
        public static string CreateKey()
        {
            return KeyGenerator.CreateKey(20);
        }
    }
}