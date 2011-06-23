using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests.Settings
{
    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void CanGetMail()
        {
            var mailSettings = Domain.Settings.Settings.Mail;
            Assert.That(mailSettings, Is.Not.Null);
        }

        [Test]
        public void CanGetCommon()
        {
            var commonSettings = Domain.Settings.Settings.Common;
            Assert.That(commonSettings, Is.Not.Null);
        }
    }
}