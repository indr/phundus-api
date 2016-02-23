namespace Phundus.Specs.Features.Account
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using NUnit.Framework;
    using Services;
    using Services.Entities;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class AccountSteps : AppStepsBase
    {
        public AccountSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I signed up")]
        public void GivenISignedUp()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [Given(@"I validated the key")]
        public void GivenIValidatedTheKey()
        {
            App.ValidateKey(Ctx.ValidationKey);
        }

        [Given(@"I changed my email address")]
        public void GivenIChangedMyEmailAddress()
        {
            ChangeEmailAddress(Ctx.User);
        }

        [Given(@"I changed email address to ""(.*)""")]
        public void GivenIChangedEmailAddressTo(string emailKey)
        {
            var user = Ctx.User;
            if (!Ctx.EmailAddresses.ContainsAlias(emailKey))
                Ctx.EmailAddresses[emailKey] = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            var emailAddress = Ctx.EmailAddresses[emailKey];
            ChangeEmailAddress(user, emailAddress);
        }

        [Given(@"I signed up and confirmed my email address")]
        public void GivenISignedUpAndConfirmedMyEmailAddress()
        {
            Given("a confirmed user");
        }

        [Given(@"an administrator locked my account")]
        public void GivenAnAdministratorLockedMyAccount()
        {
            App.LockUser(Ctx.User.UserId);
        }

        [Given(@"an administrator unlocked my account")]
        public void GivenAnAdministratorUnlockedMyAccount()
        {
            App.UnlockUser(Ctx.User.UserId);
        }

        [Given(@"I changed my password")]
        public void GivenIChangedMyPassword()
        {
            var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            var user = Ctx.User;
            App.ChangePassword(user.UserId, user.Password, newPassword);
            user.Password = newPassword;
        }

        [When(@"I try to login with my new password")]
        public void WhenITryToLoginWithMyNewPassword()
        {
            WhenITryToLogIn();
        }

        [When(@"I try to login with my old password")]
        public void WhenITryToLoginWithMyOldPassword()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.Username, user.OldPassword, false);
        }

        [When(@"I try to log in")]
        public void WhenITryToLogIn()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.Username, user.Password, false);
        }

        [When(@"I try to login with an unknown username")]
        public void WhenITryToLoginWithAnUnknownUsername()
        {
            Ctx.LoggedIn = App.LogIn("unknown@domain.com", "1234", false);
        }

        [When(@"I try to reset user's password")]
        public void WhenITryToResetPasswordUserSPassword()
        {
            App.ResetPassword(Ctx.User.Username);
        }

        [When(@"I try to sign up")]
        public void WhenITryToSignUp()
        {
            var user = App.SignUpUser();
            Ctx.User = user;
        }

        [When(@"I try to sign up with membership")]
        public void WhenITryToSignUpWithMembership()
        {
            var user = App.SignUpUser(organizationId: Ctx.Organization.OrganizationId);
            Ctx.User = user;
        }

        [When(@"I try to sign up with ""(.*)""")]
        public void WhenITryToSignUpWith(string emailAddress)
        {
            App.SignUpUser(emailAddress: emailAddress, assertHttpStatus: false);
        }

        [When(@"I try to change my email address")]
        public void WhenIChangeEmailAddress()
        {
            ChangeEmailAddress(Ctx.User);
        }

        [When(@"I try to change my email address to ""(.*)""")]
        public void WhenITryToChangeMyEmailAddressTo(string emailKey)
        {
            var user = Ctx.User;
            var emailAddress = Ctx.EmailAddresses[emailKey];

            ChangeEmailAddress(user, emailAddress, false);
        }

        [When(@"I try to log in with requested address")]
        public void WhenITryToLogInWithRequestedAddress()
        {
            var user = Ctx.User;
            Ctx.LoggedIn = App.LogIn(user.RequestedEmailAddress, user.Password, false);
        }

        [When(@"I try to validate the key")]
        public void WhenITryToValidateTheKey()
        {
            Debug.WriteLine(String.Format("Validating key {0}", Ctx.ValidationKey));
            App.ValidateKey(Ctx.ValidationKey, false);
        }

        [Then(@"I should be logged in")]
        public void ThenIShouldBeLoggedIn()
        {
            Assert.That(Ctx.LoggedIn, Is.EqualTo(Ctx.User.UserId), "User not logged in.");
        }

        [Then(@"I should not be logged in")]
        public void ThenIShouldNotBeLoggedIn()
        {
            if (Ctx.User == null)
                return;
            Assert.That(Ctx.LoggedIn, Is.Not.EqualTo(Ctx.User.UserId));
        }

        private void ChangeEmailAddress(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var newEmailAddress = Guid.NewGuid().ToString("N").Substring(0, 8) + "@test.phundus.ch";
            ChangeEmailAddress(user, newEmailAddress);
        }

        private void ChangeEmailAddress(User user, string newEmailAddress, bool assertStatusCode = true)
        {
            if (user == null) throw new ArgumentNullException("user");

            App.ChangeEmailAddress(user.UserId, user.Password, newEmailAddress, assertStatusCode);
            user.RequestedEmailAddress = newEmailAddress;
        }
    }
}