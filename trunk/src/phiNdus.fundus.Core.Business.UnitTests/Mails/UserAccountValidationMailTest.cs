using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Mails;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Mails
{
    [TestFixture]
    public class UserAccountValidationMailTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            IoC.Initialize(new WindsorContainer());
            MockFactory = new MockRepository();

            MockMailGateway = MockFactory.StrictMock<IMailGateway>();
            IoC.Container.Register(Component.For<IMailGateway>().Instance(MockMailGateway));

            User = new User();
            User.FirstName = "Ted";
            User.LastName = "Mosby";
            User.Membership.Email = "ted.mosby@example.com";
        }

        #endregion

        protected IMailGateway MockMailGateway { get; set; }

        protected MockRepository MockFactory { get; set; }

        private User User { get; set; }

        private const string ExpectedSubject = "[fundus] User Account Validation";

        private const string ExpectedBody =
            "Hello Ted\r\n\r\nPlease go to the following link in order to validate your account:\r\n[Link.UserAccountValidation]\r\n\r\nThanks\r\n\r\n--\r\nThis is automatically generated message from fundus.\r\n-\r\nIf you think it was sent incorrectly contact the administrator at admin@example.com.";

        [Test]
        public void Send_calls_gateway()
        {
            using (MockFactory.Record())
            {
                Expect.Call(() => MockMailGateway.Send("ted.mosby@example.com", ExpectedSubject, ExpectedBody));
            }

            using (MockFactory.Playback())
            {
                var mail = new UserAccountValidationMail();
                mail.Send(User);
            }
        }
    }
}