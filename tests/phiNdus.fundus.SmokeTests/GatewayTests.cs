using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business;
using phiNdus.fundus.TestHelpers;
using Rhino.Commons;

namespace phiNdus.fundus.SmokeTests
{
    [TestFixture]
    public class GatewayTests
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(new Installer());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }

        [Test]
        public void Can_send_email()
        {
            var appSettings = new System.Configuration.AppSettingsReader();
            var address = appSettings.GetValue("email.address", typeof (string)).ToString();
            var server = appSettings.GetValue("email.server", typeof (string)).ToString();
            var username = appSettings.GetValue("email.username", typeof (string)).ToString();
            var password = appSettings.GetValue("email.password", typeof (string)).ToString();

            var subject = Guid.NewGuid().ToString();

            using (var pop3 = new Pop3()) {
                pop3.Connect(server, username, password);
                pop3.DeleteAll();
            }
            
            var gateway = IoC.Resolve<IMailGateway>();
            gateway.Send(address, subject, "-- END --");

            using (var pop3 = new Pop3())
            {
                pop3.Connect(server, username, password);
                var msg = pop3.Find(subject);
                Assert.That(msg, Is.Not.Null);
            }
        }
    }
}
