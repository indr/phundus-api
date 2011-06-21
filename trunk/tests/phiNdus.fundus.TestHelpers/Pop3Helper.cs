using System;
using System.Configuration;
using System.Threading;
using NUnit.Framework;
using OpenPop.Pop3;

namespace phiNdus.fundus.TestHelpers
{
    public class Pop3Helper
    {
        private readonly string _password;
        private readonly string _server;
        private readonly string _username;
        protected TimeSpan DelayPeriod;
        protected TimeSpan Pause;

        public Pop3Helper()
        {
            DelayPeriod = TimeSpan.FromSeconds(15);
            Pause = TimeSpan.FromSeconds(1);

            var appSettings = new AppSettingsReader();
            Address = appSettings.GetValue("email.address", typeof (string)).ToString();
            _server = appSettings.GetValue("email.server", typeof (string)).ToString();
            _username = appSettings.GetValue("email.username", typeof (string)).ToString();
            _password = appSettings.GetValue("email.password", typeof (string)).ToString();

            using (var client = ConnectAndAuthenticate())
                client.DeleteAllMessages();
        }

        private Pop3Client ConnectAndAuthenticate()
        {
            var result = new Pop3Client();
            result.Connect(_server, 110, false);
            result.Authenticate(_username, _password);
            return result;
        }

        public string Address { get; set; }


        public void ConfirmEmailWasReceived(string subject)
        {
            var start = DateTime.Now;
            do
            {
                using (var client = ConnectAndAuthenticate())
                {
                    var count = client.GetMessageCount();
                    for (int i = 1; i <= count ; i++)
                    {
                        var msg = client.GetMessageHeaders(i);
                        if (msg.Subject.Equals(subject))
                            return;
                    }
                    
                }
                Thread.Sleep(Pause);
            } while (DateTime.Now < start + DelayPeriod);

            Assert.Fail("No email was found in a sensible time");
        }
    }
}