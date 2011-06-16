﻿using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Settings
{
    [TestFixture]
    public class SmtpSettingsTests : BaseTestFixture
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

        [Test]
        public void Get_From()
        {
            var value = Sut.From;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("fundus-sys-test-1@indr.ch"));
        }

        [Test]
        public void Get_UserName()
        {
            var value = Sut.UserName;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("fundus-sys-test-1@indr.ch"));
        }

        [Test]
        public void Get_Password()
        {
            var value = Sut.Password;
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo("phiNdus"));
        }
    }
}