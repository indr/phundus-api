namespace phiNdus.fundus.Domain
{
    public static class PasswordGenerator
    {
        public static string CreatePassword()
        {
            return KeyGenerator.CreateKey(8);
        }
    }
}