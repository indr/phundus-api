namespace Phundus.IdentityAccess.Users.Services
{
    public static class PasswordGenerator
    {
        public static string CreatePassword()
        {
            return KeyGenerator.CreateKey(8);
        }
    }
}