using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Rhino.Commons;

namespace phiNdus.fundus.Domain
{
    public static class PasswordEncryptor
    {
        public static string Encrypt(string value, string salt)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (salt == null)
                throw new ArgumentNullException("salt");
            
            var md5 = new MD5CryptoServiceProvider();
            var hashed = md5.ComputeHash(new UnicodeEncoding().GetBytes(salt + value));
            return BitConverter.ToString(hashed).Replace("-", "").ToLower(CultureInfo.CurrentCulture);
        }
    }
}