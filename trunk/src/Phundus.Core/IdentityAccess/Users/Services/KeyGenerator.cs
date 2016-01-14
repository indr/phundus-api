namespace Phundus.IdentityAccess.Users.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    public static class KeyGenerator
{
        public static string CreateKey(int length)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[(length/2) + 1];
            rng.GetBytes(buff);
            return BytesToHexString(buff, length);
        }

        private static string BytesToHexString(ICollection<byte> bytes, int length)
        {
            var builder = new StringBuilder(bytes.Count);
            foreach (var each in bytes)
                builder.Append(String.Format(CultureInfo.CurrentCulture, "{0:X2}", each));
            return builder.ToString().ToLower(CultureInfo.CurrentCulture).Substring(0, length);
        }
    }
}