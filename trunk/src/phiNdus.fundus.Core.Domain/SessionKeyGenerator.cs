using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace phiNdus.fundus.Core.Domain
{
    public class SessionKeyGenerator
    {
        public static string CreateKey()
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[10];
            rng.GetBytes(buff);
            return BytesToHexString(buff);
        }

        private static string BytesToHexString(byte[] bytes)
        {
            var builder = new StringBuilder(20);
            foreach (var each in bytes)
                builder.Append(String.Format(CultureInfo.CurrentCulture, "{0:X2}", each));
            return builder.ToString().ToLower(CultureInfo.CurrentCulture);
        }
    }
}