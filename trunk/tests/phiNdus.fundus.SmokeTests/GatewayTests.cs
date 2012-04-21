using System;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Business;
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
        public void CanSendEmail()
        {
            var subject = Guid.NewGuid().ToString();
            var pop3Helper = new Pop3Helper();
            var gateway = IoC.Resolve<IMailGateway>();

            gateway.Send(pop3Helper.Address, subject, "-- END --");

            pop3Helper.ConfirmEmailWasReceived(subject);
        }
    }
}