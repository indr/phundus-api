namespace phiNdus.fundus.Domain.UnitTests.Settings
{
    using Domain.Settings;
    using NUnit.Framework;

    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void CanGetMail()
        {
            var mailSettings = Settings.Mail;
            Assert.That(mailSettings, Is.Not.Null);
        }
    }
}