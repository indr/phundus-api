namespace phiNdus.fundus.Domain.UnitTests.Settings
{
    using Domain.Settings;
    using NUnit.Framework;

    [TestFixture]
    public class MailSettingsTest
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
        public void Can_get_Templates()
        {
            var templatesSettings = Sut.TemplatesSettings;
            Assert.That(templatesSettings, Is.Not.Null);
        }
    }
}