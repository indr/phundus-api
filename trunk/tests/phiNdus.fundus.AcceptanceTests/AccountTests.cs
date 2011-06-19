using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business;
using phiNdus.fundus.Core.Business.Services;
using Rhino.Commons;
using WatiN.Core;

namespace phiNdus.fundus.AcceptanceTests
{
    [TestFixture]
    public class AccountTests
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(new Installer());

            var appSettings = new System.Configuration.AppSettingsReader();
            BaseUri = appSettings.GetValue("uri", typeof(string)).ToString();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }

        protected string BaseUri { get; private set; }

        [Test]
        public void SignUp()
        {
            Assert.Ignore("Not implemented");

            // Given a user with email dave@example.com
            // When I sign up with dave@example.com
            // Then I see that email is already taken
            
            var userService = new UserService();
            if (userService.GetUser("dave@example.com") == null)
                userService.CreateUser("dave@example.com", "password");

            using (var browser = new IE(BaseUri))
            {
                browser.GoTo(BaseUri + "/Account/SignUp");
                browser.TextField(Find.ById("Email")).TypeText("dave@example.com");
                browser.Button(Find.ByValue("Registrieren")).Click();

                Assert.IsTrue(browser.ContainsText("Die E-Mail-Adresse wird bereits verwendet"));
            }
        }

        [Test]
        public void SignUpShowsRequiredFields()
        {
            // When I sign up without filling fields
            // Then I see which fields are required

            using (var browser = new IE(BaseUri + "/Account/SignUp"))
            {
                browser.Button(Find.ByValue("Registrieren")).Click();

                Assert.IsTrue(browser.ContainsText("The Vorname field is required."));
                Assert.IsTrue(browser.ContainsText("The Nachname field is required."));
                Assert.IsTrue(browser.ContainsText("The E-Mail-Adresse field is required."));
            }
            
        }

        [Test]
        public void SignUpShowsInvalidEmail()
        {
            // When I sign up with invalid email
            // Then I see invalid email

            using (var browser = new IE(BaseUri + "/Account/SignUp"))
            {
                browser.TextField(Find.ById("Email")).TypeText("abc.com");
                browser.Button(Find.ByValue("Registrieren")).Click();

                Assert.IsTrue(browser.ContainsText("Ungültige E-Mail-Adresse"));
            }
        }
    }
}