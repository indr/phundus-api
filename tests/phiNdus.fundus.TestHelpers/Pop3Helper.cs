using System;
using System.Configuration;
using System.Threading;
using NUnit.Framework;

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
            DelayPeriod = TimeSpan.FromSeconds(10);
            Pause = TimeSpan.FromSeconds(1);

            var appSettings = new AppSettingsReader();
            Address = appSettings.GetValue("email.address", typeof (string)).ToString();
            _server = appSettings.GetValue("email.server", typeof (string)).ToString();
            _username = appSettings.GetValue("email.username", typeof (string)).ToString();
            _password = appSettings.GetValue("email.password", typeof (string)).ToString();

            using (var pop3 = new Pop3())
            {
                pop3.Connect(_server, _username, _password);
                pop3.DeleteAll();
            }
        }


        public string Address { get; set; }


        public void ConfirmEmailWasReceived(string subject)
        {
            var start = DateTime.Now;
            do
            {
                using (var pop3 = new Pop3())
                {
                    pop3.Connect(_server, _username, _password);
                    Pop3Message msg = pop3.Find(subject);
                    if (msg != null)
                        return;
                }
                Thread.Sleep(Pause);
            } while (DateTime.Now < start + DelayPeriod);

            Assert.Fail("No email was found in a sensible time");
        }
    }
}