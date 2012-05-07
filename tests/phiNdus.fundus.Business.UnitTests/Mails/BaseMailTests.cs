using NUnit.Framework;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Business.Mails;
using phiNdus.fundus.Domain.Settings;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.Mails
{
    [TestFixture]
    public class BaseMailTests : UnitTestBase<object>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            Assert.Ignore("TODO");
            base.SetUp();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();
            MockMailTemplateSettings = Obsolete_MockFactory.StrictMock<IMailTemplateSettings>();
            MockSettings = Obsolete_CreateAndRegisterStrictMock<ISettings>();
            Settings.SetGlobalNonThreadSafeSettings(MockSettings);
        }

        [TearDown]
        public override void TearDown()
        {
            Settings.SetGlobalNonThreadSafeSettings(null);
            base.TearDown();
        }

        #endregion

        protected ISettings MockSettings { get; set; }

        private class HackedBaseMail : BaseMail
        {
            public HackedBaseMail(IMailTemplateSettings settings) : base(settings)
            {
            }

            public void Send()
            {
                base.Send("test@example.com");
            }

            public void AddData(string name, object data)
            {
                // TODO
                //base.DataContext.Add(name, data);
            }
        }

        protected IMailGateway MockMailGateway { get; set; }

        protected IMailTemplateSettings MockMailTemplateSettings { get; set; }


        private class NullTestData
        {
            public string ImNull
            {
                get { return null; }
            }
        }

        private class RecursiveTestData
        {
            public string FirstVariable
            {
                get { return "I'm First and next says [Test.SecondVariable]"; }
            }

            public string SecondVariable
            {
                get { return "I'm Second and next says [Test.ThirdVariable]"; }
            }

            public string ThirdVariable
            {
                get { return "I'm Thrid"; }
            }
        }

        [Test]
        public void ReplacesNullVariableWithEmptyString()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockMailTemplateSettings.Subject).Return("Subject");
                Expect.Call(MockMailTemplateSettings.TextBody).Return("Start[Test.ImNull]End");
                Expect.Call(MockSettings.Common.ServerUrl).Return("fundus.domain.test");

                Expect.Call(() => MockMailGateway.Send(Arg<string>.Is.Anything, Arg<string>.Is.Anything,
                                                       Arg<string>.Matches(y => y.StartsWith("StartEnd"))));
            }

            using (Obsolete_MockFactory.Playback())
            {
                var sut = new HackedBaseMail(MockMailTemplateSettings);
                sut.AddData("Test", new NullTestData());
                sut.Send();
            }
        }

        [Test]
        public void ReplacesUserAccountValidationLink()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockMailTemplateSettings.Subject).Return("Subject");
                Expect.Call(MockMailTemplateSettings.TextBody).Return("[Link.UserAccountValidation]");
                Expect.Call(MockSettings.Common.ServerUrl).Return("fundus.domain.test");

                Expect.Call(() => MockMailGateway.Send(Arg<string>.Is.Anything, Arg<string>.Is.Anything,
                    Arg<string>.Matches(y => y.StartsWith("http://fundus.domain.test/Account/Validation/[Membership.ValidationKey]"))));
            }

            using (Obsolete_MockFactory.Playback())
            {
                var sut = new HackedBaseMail(MockMailTemplateSettings);
                sut.Send();
            }
        }

        [Test]
        public void ReplacesVariablesRecursivTwoTimes()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockMailTemplateSettings.Subject).Return("Subject");
                Expect.Call(MockMailTemplateSettings.TextBody).Return("[Test.FirstVariable]");
                Expect.Call(MockSettings.Common.ServerUrl).Return("fundus.domain.test");

                Expect.Call(() => MockMailGateway.Send(Arg<string>.Is.Anything, Arg<string>.Is.Anything,
                                                       Arg<string>.Matches(
                                                           y => y.StartsWith("I'm First and next says I'm Second"))));
            }

            using (Obsolete_MockFactory.Playback())
            {
                var sut = new HackedBaseMail(MockMailTemplateSettings);
                sut.AddData("Test", new RecursiveTestData());
                sut.Send();
            }
        }
    }
}