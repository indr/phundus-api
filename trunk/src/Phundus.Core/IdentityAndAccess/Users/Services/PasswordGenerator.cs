namespace Phundus.Core.IdentityAndAccessCtx
{
    public static class PasswordGenerator
    {
        public static string CreatePassword()
        {
            return KeyGenerator.CreateKey(8);
        }
    }
}