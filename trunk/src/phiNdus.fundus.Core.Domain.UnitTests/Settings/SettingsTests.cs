using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests.Settings
{
    [TestFixture]
    internal class SettingsTests
    {
        [Test]
        public void Can_get_Mail()
        {
            var mailSettings = Domain.Settings.Settings.Mail;
            Assert.That(mailSettings, Is.Not.Null);
        }
    }
}