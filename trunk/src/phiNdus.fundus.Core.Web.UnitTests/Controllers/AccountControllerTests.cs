using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Web.Controllers;
using phiNdus.fundus.Core.Web.Models;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;
using models = phiNdus.fundus.Core.Web.Models;

namespace phiNdus.fundus.Core.Web.UnitTests.Controllers {

    [TestFixture]
    public class AccountControllerTests : MockTestBase<AccountController> {

        private IMembershipService MembershipServiceMock { get; set; }
        private IFormsService FormsServiceMock { get; set; }

        protected override void RegisterDependencies(IWindsorContainer container) {
            base.RegisterDependencies(container);
        
            this.MembershipServiceMock = this.MockFactory.StrictMock<IMembershipService>();
            this.FormsServiceMock = this.MockFactory.StrictMock<IFormsService>();

            container.Register(Component.For<IMembershipService>().Instance(this.MembershipServiceMock));
            container.Register(Component.For<IFormsService>().Instance(this.FormsServiceMock));
        }

        protected override AccountController CreateSut() {
            return new AccountController();
        }

        [Test]
        public void LogOn_should_return_login_page() {
            var view = (System.Web.Mvc.ViewResult)this.Sut.LogOn();
            var model = (phiNdus.fundus.Core.Web.Models.LogOnModel)view.Model;

            // ist das richtig so?
            Assert.That(model, Is.Null);
        }

        [Test]
        public void When_try_logging_in_with_an_invalid_email() {
            var email = "barney@stinson";
            var password = "no1old3RtH4n22";
            var createCookie = true;
            
            var view = this.Sut.LogOn(new LogOnModel {
                Email = email,
                Password = password,
                RememberMe = createCookie
            }, "GeheimerBereich.cshtml");

            // Todo,jac: Validieren der ViewDaten    
        }
        
        [Test]
        public void When_logging_in_with_valid_credentials() {
            var email = "barney@stinson.com";
            var password = "no1old3RtH4n22";
            var createCookie = true;

            using (this.MockFactory.Record()) {
                Expect.Call(this.MembershipServiceMock.ValidateUser(email, password))
                    .Return(true);
                Expect.Call(() => this.FormsServiceMock.SignIn(email, createCookie));
            }

            using (this.MockFactory.Playback()) {
                var view = this.Sut.LogOn(new LogOnModel {
                    Email = email,
                    Password = password,
                    RememberMe = createCookie
                }, "GeheimerBereich.cshtml");

                // Todo,jac: Validieren der ViewDaten
            }
        }

        [Test]
        public void When_logging_off_user_should_be_signed_out() {
            using (this.MockFactory.Record()) {
                Expect.Call(() => this.FormsServiceMock.SignOut());
            }

            using (this.MockFactory.Playback()) {
                var view = this.Sut.LogOff();

                // Todo,jac: Validieren der ViewDaten
            }
        }
    }
}
