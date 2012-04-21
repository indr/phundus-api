using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests.Settings
{
    [TestFixture]
    public class HowToMockSettingsTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _settings = MockRepository.GenerateMock<ISettings>();
            Domain.Settings.Settings.SetGlobalNonThreadSafeSettings(_settings);
        }

        [TearDown]
        public void TearDown()
        {
            Domain.Settings.Settings.SetGlobalNonThreadSafeSettings(null);
        }

        #endregion

        private ISettings _settings;

        [Test]
        public void CanInjectStaticSettings()
        {
            _settings.Expect(x => x.Common).Return(null);

            ICommonSettings common = Domain.Settings.Settings.Common;

            _settings.VerifyAllExpectations();
        }

        [Test]
        public void CanVerifyStaticCall()
        {
            _settings.Expect(x => x.Common).Return(null);

            var ex = Assert.Throws<Rhino.Mocks.Exceptions.ExpectationViolationException>(() => _settings.VerifyAllExpectations());

            Assert.That(ex.Message, Is.EqualTo("ISettings.get_Common(); Expected #1, Actual #0."));
        }

        [Test]
        public void CanRecursiveStubToStubMultipleProperties()
        {
            var smtpSettings = MockRepository.GenerateMock<ISmtpSettings>();
            _settings.Stub(x => x.Mail.Smtp).Return(smtpSettings);
            smtpSettings.Stub(x => x.UserName).Return("John");
            smtpSettings.Stub(x => x.Password).Return("1234");

            string userName = Domain.Settings.Settings.Mail.Smtp.UserName;
            string password = Domain.Settings.Settings.Mail.Smtp.Password;

            Assert.That(userName, Is.EqualTo("John"));
            Assert.That(password, Is.EqualTo("1234"));
        }

        [Test]
        public void CanRecursiveStubViaStubbedProperty()
        {
            _settings.Stub(x => x.Mail).Return(MockRepository.GenerateMock<IMailSettings>());
            _settings.Mail.Stub(x => x.Smtp).Return(MockRepository.GenerateMock<ISmtpSettings>());
            _settings.Mail.Smtp.Stub(x => x.UserName).Return("John");
            _settings.Mail.Smtp.Stub(x => x.Password).Return("1234");

            string userName = Domain.Settings.Settings.Mail.Smtp.UserName;
            string password = Domain.Settings.Settings.Mail.Smtp.Password;

            Assert.That(userName, Is.EqualTo("John"));
            Assert.That(password, Is.EqualTo("1234"));
        }

        [Test]
        public void CanStubSecondLevelPropertyRecursive()
        {
            _settings.Stub(x => x.Common.ServerUrl).Return("example.com");

            string url = Domain.Settings.Settings.Common.ServerUrl;

            Assert.That(url, Is.EqualTo("example.com"));
        }

        [Test]
        public void CanStubTwoSecondLevelPropertiesRecursive()
        {
            Assert.Ignore("Multiple recursive stubbing/mocking is not possible!");
            _settings.Stub(x => x.Mail.Smtp.UserName).Return("John");
            _settings.Stub(x => x.Mail.Smtp.Password).Return("1234");

            string userName = Domain.Settings.Settings.Mail.Smtp.UserName;
            string password = Domain.Settings.Settings.Mail.Smtp.Password;

            Assert.That(userName, Is.EqualTo("John"));
            Assert.That(password, Is.EqualTo("1234"));
        }

        [Test]
        public void CanVerifyExpectationOnStubbedProperty()
        {
            _settings.Stub(x => x.Mail).Return(MockRepository.GenerateMock<IMailSettings>());
            _settings.Mail.Stub(x => x.Smtp).Return(MockRepository.GenerateMock<ISmtpSettings>());
            _settings.Mail.Smtp.Stub(x => x.UserName).Return("John");
            _settings.Mail.Smtp.Stub(x => x.Password).Return("1234");

            //string userName = Domain.Settings.Settings.Mail.Smtp.UserName;
            //string password = Domain.Settings.Settings.Mail.Smtp.Password;

            var ex1 = Assert.Throws<Rhino.Mocks.Exceptions.ExpectationViolationException>(() => _settings.Mail.Smtp.AssertWasCalled(x => x.UserName));
            Assert.That(ex1.Message, Is.EqualTo("ISmtpSettings.get_UserName(); Expected #1, Actual #0."));
            var ex2 = Assert.Throws<Rhino.Mocks.Exceptions.ExpectationViolationException>(() => _settings.Mail.Smtp.AssertWasCalled(x => x.Password));
            Assert.That(ex2.Message, Is.EqualTo("ISmtpSettings.get_Password(); Expected #1, Actual #0."));
        }
    }
}