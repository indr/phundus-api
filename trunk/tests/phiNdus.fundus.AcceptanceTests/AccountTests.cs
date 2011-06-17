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
            // Given a user with email dave@example.com
            // When sign up with dave@example.com
            // Then shows that email is already taken
            
            var userService = new UserService();
            if (userService.GetUser("dave@example.com") == null)
                userService.CreateUser("dave@example.com", "password");

            using (var browser = new IE(BaseUri))
            {
                browser.GoTo(BaseUri + "/Account/SignUp");
                browser.TextField(Find.ById("Email")).TypeText("dave@example.com");
                browser.Button(Find.ByValue("Registrieren")).Click();

                // TODO,Inder: Falsche Bedingung!
                Assert.IsTrue(browser.ContainsText("Konnte Benutzer nicht erstellen"));
            }
        }
    }
}