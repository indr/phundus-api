using System;
using System.Threading;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace piNuts.phundus.Specs.Infrastructure
{
    public class Mailbox
    {
        private readonly TimeSpan _delayPeriod;
        private readonly string _hostName;
        private readonly string _password;

        private readonly TimeSpan _pause;
        private readonly string _userName;

        private Mailbox(string hostName, string userName, string password)
        {
            _hostName = hostName;
            _userName = userName;
            _password = password;

            _delayPeriod = TimeSpan.FromSeconds(20);
            _pause = TimeSpan.FromSeconds(1);
        }

        public static Mailbox For(string emailAddress)
        {
            if (emailAddress == "user@test.phundus.ch")
                return new Mailbox("phundus.ch", "user@test.phundus.ch", "p@ss_w03d");
            if (emailAddress == "admin@test.phundus.ch")
                return new Mailbox("phundus.ch", "admin@test.phundus.ch", "p@ss_w03d");
            return new Mailbox("phundus.ch", emailAddress, "p@ss_w03d");
            //throw new ArgumentOutOfRangeException(
            //    String.Format("Die Login-Daten zur E-Mail-Adresse \"{0}\" sind nicht bekannt."));
        }

        private Pop3Client ConnectAndAuthenticate()
        {
            var result = new Pop3Client();
            result.Connect(_hostName, 110, false);
            result.Authenticate(_userName, _password);
            return result;
        }

        public Message Find(string subject)
        {
            var start = DateTime.Now;
            do
            {
                using (var client = ConnectAndAuthenticate())
                {
                    try
                    {
                        var count = client.GetMessageCount();
                        for (var i = 1; i <= count; i++)
                        {
                            var msg = client.GetMessageHeaders(i);
                            if (msg.Subject.Equals(subject))
                                return client.GetMessage(i);
                        }
                    }
                    finally
                    {
                        client.DeleteAllMessages();
                    }
                }
                Thread.Sleep(_pause);
            } while (DateTime.Now < start + _delayPeriod);
            return null;
        }
    }
}