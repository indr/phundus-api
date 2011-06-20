using System.Threading;
using NUnit.Framework;
using phiNdus.fundus.AcceptanceTests.AppDriver;
using phiNdus.fundus.AcceptanceTests.AppDriver.WindowDriver;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.TestHelpers;
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
        public void SignUpSendsValidationEmail()
        {
            // When I sign up
            // Then I receive email with subject [fundus] User Account Validation

            var pop3Helper = new Pop3Helper();
            var adminApi = new AdminApi();
            adminApi.DeleteUser(pop3Helper.Address);

            var signUpWindow = new SignUpWindowDriver(Context);
            signUpWindow.SpecifyFirstName("Hans");
            signUpWindow.SpecifyLastName("Muster");
            signUpWindow.SpecifyEmail(pop3Helper.Address);
            signUpWindow.SpecifyPassword("123qwe");
            signUpWindow.SignUp();

            pop3Helper.ConfirmEmailWasReceived("[fundus] User Account Validation");
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
            // Then I see Das Feld "Vorname" ist erforderlich.
            //  and I see Das Feld "Nachname" ist erforderlich.
            //  and I see Das Feld "E-Mail-Adresse" ist erforderlich.
            //  and I see Das Feld "Passwort" ist erforderlich.

            var signUpWindow = new SignUpWindowDriver(Context);
            signUpWindow.SignUp();

            signUpWindow.ContainsText(@"Das Feld ""Vorname"" ist erforderlich.");
            signUpWindow.ContainsText(@"Das Feld ""Nachname"" ist erforderlich.");
            signUpWindow.ContainsText(@"Das Feld ""E-Mail-Adresse"" ist erforderlich.");
            signUpWindow.ContainsText(@"Das Feld ""Passwort"" ist erforderlich.");
        }
    }
}