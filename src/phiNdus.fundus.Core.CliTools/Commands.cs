using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace phiNdus.fundus.Core.CliTools
{
    public class Commands
    {
        public void Encrypt(TextWriter writer, string[] args)
        {
            if (args.Length != 3)
            {
                writer.WriteLine("Command: encrypt need 2 parameters: password, salt");
                return;
            }

            // TODO,Inder: Irgendwie bring ich die Assembly-Referenz nicht hin?!
            //var encrypted = phiNdus.fundus.Core.Domain.PasswordEncryptor.Encrypt(args[1], args[2]);
            var encrypted = CopyOfPasswordEncryptor.Encrypt(args[1], args[2]);

            Console.WriteLine("Password: " + args[1]);
            Console.WriteLine("Salt: " + args[2]);
            Console.WriteLine("Encrypted: " + encrypted);
            Console.WriteLine("");
        }

        public void CreateSessionKey(TextWriter writer, string[] args)
        {
            Console.WriteLine("Session key: " + CopyOfKeyGenerator.CreateKey(20));
            Console.WriteLine("");
        }
    }

    class CopyOfPasswordEncryptor
    {
        public static string Encrypt(string value, string salt)
        {
            var md5 = new MD5CryptoServiceProvider();
            var hashed = md5.ComputeHash(new UnicodeEncoding().GetBytes(salt + value));
            return BitConverter.ToString(hashed).Replace("-", "").ToLower(CultureInfo.CurrentCulture);
        }
    }

    class CopyOfKeyGenerator
    {
        public static string CreateKey(int length)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[(length / 2) + 1];
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