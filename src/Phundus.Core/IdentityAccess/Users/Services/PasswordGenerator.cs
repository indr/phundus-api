namespace Phundus.Core.IdentityAndAccess.Users.Services
{
    public static class PasswordGenerator
    {
        public static string CreatePassword()
        {
            return KeyGenerator.CreateKey(8);
        }
    }
}