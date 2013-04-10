using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Web.Controllers;
using phiNdus.fundus.Web.Models;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;
using models = phiNdus.fundus.Web.Models;

namespace phiNdus.fundus.Web.UnitTests.Controllers {

    [TestFixture]
    public class AccountControllerTests : MockTestBase<AccountController> {

        private IMembershipService MembershipServiceMock { get; set; }
        

        protected override void RegisterDependencies(IWindsorContainer container) {
            base.RegisterDependencies(container);
        
            this.MembershipServiceMock = this.MockFactory.StrictMock<IMembershipService>();

            container.Register(Component.For<IMembershipService>().Instance(this.MembershipServiceMock));
        }

        protected override AccountController CreateSut() {
            return new AccountController();
        }

        [Test]
        public void LogOn_should_return_login_page() {
            var view = (System.Web.Mvc.ViewResult)this.Sut.LogOn();
            var model = (phiNdus.fundus.Web.Models.LogOnModel)view.Model;

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
        
        

        
    }
}
