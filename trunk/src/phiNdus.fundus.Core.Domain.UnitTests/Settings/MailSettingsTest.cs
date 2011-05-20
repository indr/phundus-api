using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Domain.UnitTests.Settings
{
    [TestFixture]
    internal class MailSettingsTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new MailSettings("");
        }

        #endregion

        protected MailSettings Sut { get; set; }

        [Test]
        public void Can_get_Smtp()
        {
            var smtpSettings = Sut.Smtp;
            Assert.That(smtpSettings, Is.Not.Null);
        }

        [Test]
        public void Can_get_Templates()
        {
            var templatesSettings = Sut.TemplatesSettings;
            Assert.That(templatesSettings, Is.Not.Null);
        }
    }
}