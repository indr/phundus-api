namespace Phundus.Core.IdentityAndAccess.Users.Services
{
    public static class SessionKeyGenerator
    {
        public static string CreateKey()
        {
            return KeyGenerator.CreateKey(20);
        }
    }
}