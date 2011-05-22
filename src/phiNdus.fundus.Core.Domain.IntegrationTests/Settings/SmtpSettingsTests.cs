using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Settings
{
    [TestFixture]
    internal class SmtpSettingsTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = Domain.Settings.Settings.Mail.Smtp;
        }

        #endregion

        protected ISmtpSettings Sut { get; set; }

        [Test]
        public void Get_Host()
        {
            var value = Sut.Host;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("mail.indr.ch"));
        }
    }
}