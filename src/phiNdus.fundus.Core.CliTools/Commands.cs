using System.IO;
using phiNdus.fundus.Core.Domain;

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

            var encrypted = PasswordEncryptor.Encrypt(args[1], args[2]);

            writer.WriteLine("Password: " + args[1]);
            writer.WriteLine("Salt: " + args[2]);
            writer.WriteLine("Encrypted: " + encrypted);
            writer.WriteLine("");
        }
    }
}