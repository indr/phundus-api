using NUnit.Framework;
using phiNdus.fundus.AcceptanceTests.AppDriver.WindowDriver;
using phiNdus.fundus.Core.Business.Services;
using WatiN.Core;

namespace phiNdus.fundus.AcceptanceTests
{
    [TestFixture]
    public class AccountTests : DslTestCase
    {
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
        public void SignUpShowsInvalidEmail()
        {
            // When I sign up with invalid email
            // Then I see Ungültige E-Mail-Adresse

            var signUpWindow = new SignUpWindowDriver(Context);
            signUpWindow.SpecifyEmail("abc.com");
            signUpWindow.SignUp();

            signUpWindow.ContainsText("Ungültige E-Mail-Adresse");
        }

        [Test]
        public void SignUpShowsRequiredFields()
        {
            // When I sign up
            // Then I see The Vorname field is required.
            //  and I see The Nachname field is required.
            //  and I see The E-Mail-Adresse field is required.

            var signUpWindow = new SignUpWindowDriver(Context);
            signUpWindow.SignUp();

            signUpWindow.ContainsText("The Vorname field is required.");
            signUpWindow.ContainsText("The Nachname field is required.");
            signUpWindow.ContainsText("The E-Mail-Adresse field is required.");
        }
    }
}