using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Settings
{
    [TestFixture]
    public class SmtpSettingsTests : SettingsTestFixture<ISmtpSettings>
    {
        [Test]
        public void GetHost()
        {
            InsertSetting("mail.smtp.host", "mail.domain.ch");

            var value = Sut.Host;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("mail.domain.ch"));
        }

        [Test]
        public void GetFrom()
        {
            InsertSetting("mail.smtp.from", "no-reply@domain.ch");

            var value = Sut.From;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("no-reply@domain.ch"));
        }

        [Test]
        public void GetUserName()
        {
            InsertSetting("mail.smtp.user-name", "no-reply");

            var value = Sut.UserName;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("no-reply"));
        }

        [Test]
        public void GetPassword()
        {
            InsertSetting("mail.smtp.password", "secret");

            var value = Sut.Password;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("secret"));
        }

        protected override ISmtpSettings CreateSut()
        {
            return new SettingsImpl().Mail.Smtp;
        }
    }
}