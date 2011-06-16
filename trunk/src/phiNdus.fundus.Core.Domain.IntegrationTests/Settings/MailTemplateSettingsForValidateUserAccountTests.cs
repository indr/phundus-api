using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Settings
{
    [TestFixture]
    public class MailTemplateSettingsForValidateUserAccountTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = Domain.Settings.Settings.Mail.Templates.UserAccountValidation;
        }

        #endregion

        private IMailTemplateSettings Sut { get; set; }

        [Test]
        public void Can_get_Body()
        {
            var actual = Sut.Body;
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.StringStarting("Hello [User.FirstName]"));
        }

        [Test]
        public void Can_get_Subject()
        {
            var actual = Sut.Subject;
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo("[fundus] User Account Validation"));
        }
    }
}