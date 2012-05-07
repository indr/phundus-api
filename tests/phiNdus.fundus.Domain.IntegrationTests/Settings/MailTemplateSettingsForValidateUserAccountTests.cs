using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Settings;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.IntegrationTests.Settings
{
    [TestFixture]
    public class MailTemplateSettingsForValidateUserAccountTests : SettingsTestFixture<IMailTemplateSettings>
    {
        [Test]
        public void GetBody()
        {
            InsertSetting("mail.templates.user-account-validation.body", "Hello");

            var actual = Sut.TextBody;
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.StringStarting("Hello"));
        }

        [Test]
        public void GetSubject()
        {
            InsertSetting("mail.templates.user-account-validation.subject", "Subject");

            var actual = Sut.Subject;
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo("Subject"));
        }

        protected override IMailTemplateSettings CreateSut()
        {
            return new SettingsImpl().Mail.Templates.UserAccountValidation;
        }
    }
}