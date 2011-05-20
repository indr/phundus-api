using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Settings
{
    [TestFixture]
    class MailTemplateSettingsForValidateUserAccountTests : UnitOfWorkEnsuredTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Sut = Domain.Settings.Settings.Mail.Templates.UserAccountValidation;
        }

        private IMailTemplateSettings Sut { get; set; }

        [Test]
        public void Can_get_Subject()
        {
            var actual = Sut.Subject;
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo("[fundus] User Account Validation"));
        }

        [Test]
        public void Can_get_Body()
        {
            var actual = Sut.Body;
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.StringStarting("Hello [User.FirstName]"));
        }
    }
}
