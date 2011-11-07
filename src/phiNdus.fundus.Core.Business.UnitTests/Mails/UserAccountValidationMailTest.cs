using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Mails;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Settings;
using phiNdus.fundus.TestHelpers;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Mails
{
    [TestFixture]
    public class UserAccountValidationMailTest : UnitTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            MockFactory = new MockRepository();
            
            
            RegisterDependencies(IoC.Container);

        }

        private MockRepository MockFactory { get; set; }

        protected void RegisterDependencies(IWindsorContainer container)
        {
            MockMailGateway = MockFactory.StrictMock<IMailGateway>();
            IoC.Container.Register(Component.For<IMailGateway>().Instance(MockMailGateway));

            MockSettings = MockFactory.Stub<ISettings>();
            MockMailTemplateSettings = MockFactory.Stub<IMailTemplateSettings>();
            MockCommonSettings = MockFactory.Stub<ICommonSettings>();

            User = new User();
            User.FirstName = "Ted";
            User.LastName = "Mosby";
            User.Membership.Email = "ted.mosby@example.com";
            User.Membership.GenerateValidationKey();
        }

        protected ICommonSettings MockCommonSettings { get; set; }

        private void RecordSettings()
        {
            Settings.SetGlobalNonThreadSafeSettings(MockSettings);
            Expect.Call(MockSettings.Mail.Templates.UserAccountValidation)
                .Return(MockMailTemplateSettings).Repeat.Any();
            Expect.Call(MockSettings.Common)
                .Return(MockCommonSettings).Repeat.Any();

            Expect.Call(MockMailTemplateSettings.Subject)
                .Return("[fundus] User Account Validation").Repeat.Any();
            Expect.Call(MockMailTemplateSettings.Body)
                .Return("Hello [User.FirstName]\r\n\r\nPlease go to the following link in order to validate your account:\r\n[Link.UserAccountValidation]\r\n\r\nThanks")
                .Repeat.Any();
            Expect.Call(MockCommonSettings.ServerUrl).Return("fundus.domain.com").Repeat.Any();
        }

        private IMailTemplateSettings MockMailTemplateSettings { get; set; }
        private IMailGateway MockMailGateway { get; set; }
        private ISettings MockSettings { get; set; }

        [TearDown]
        public override void TearDown()
        {
            Settings.SetGlobalNonThreadSafeSettings(null);
        }

        private User User { get; set; }

        [Test]
        public void CtorReadsSettings()
        {
            using (MockFactory.Record())
            {
                RecordSettings();
            }

            using (MockFactory.Playback())
            {
                var sut = new UserAccountValidationMail();
                Assert.That(sut.Subject, Is.EqualTo(TemplateSubject));
                Assert.That(sut.Body, Is.EqualTo(TemplateBody));
            }
        }

        [Test]
        public void ForReturnsSameInstance()
        {
            using (MockFactory.Record())
            {
                RecordSettings();
            }

            using (MockFactory.Playback())
            {
                var sut = new UserAccountValidationMail();
                Assert.That(sut.For(User), Is.SameAs(sut));
            }
        }

        [Test]
        public void ForWithNullUserThrows()
        {
            using (MockFactory.Record())
            {
                RecordSettings();
            }

            using (MockFactory.Playback())
            {
                var sut = new UserAccountValidationMail();
                var ex = Assert.Throws<ArgumentNullException>(() => sut.For(null));
                Assert.That(ex.ParamName, Is.EqualTo("user"));
            }
            
        }

        [Test]
        public void SendRelaysToGateway()
        {
            using (MockFactory.Record())
            {
                RecordSettings();
                Expect.Call(() => MockMailGateway.Send("ted.mosby@example.com", ReplacedSubject, ReplacedBody));
            }

            using (MockFactory.Playback())
            {
                var sut = new UserAccountValidationMail();
                sut.For(User);
                sut.Send(User);
            }
        }

        private const string TemplateSubject = "[fundus] User Account Validation";
        private const string ReplacedSubject = "[fundus] User Account Validation";
        private const string TemplateBody = @"Hello [User.FirstName]

Please go to the following link in order to validate your account:
[Link.UserAccountValidation]

Thanks";
        private string ReplacedBody
        {
            get
            {
                return
                    @"Hello Ted

Please go to the following link in order to validate your account:
" + "http://fundus.domain.com/Account/Validation/" + User.Membership.ValidationKey + @"

Thanks

--
This is automatically generated message from fundus.
-
If you think it was sent incorrectly contact the administrator at admin@example.com.";
            }
        }
    }
}