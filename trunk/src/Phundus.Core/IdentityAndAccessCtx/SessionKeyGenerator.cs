namespace Phundus.Core.IdentityAndAccessCtx
{
    public static class SessionKeyGenerator
    {
        public static string CreateKey()
        {
            return KeyGenerator.CreateKey(20);
        }
    }
}